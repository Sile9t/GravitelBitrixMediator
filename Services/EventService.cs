using Entities.Dtos.Bitrix;
using Entities.Dtos.Gravitel;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class EventService : IEventService
    {
        IRepositoryManager _repo;

        public EventService(IRepositoryManager repo)
        {
            _repo = repo;
        }

        public async Task HandleEvent(EventInfoDto eventInfo)
        {
            //Ищем Id пользователя
            var assignedUsers = _repo.Contact
                .GetContactsByFilter($"'PHONE' => {eventInfo.Phone}")!.Result;
            var clientContact = _repo.Contact
                .GetContactsByFilter($"'PHONE' => {eventInfo.Client}")!.Result;
            //Ищем сделки клиента
            var deals = _repo.Deal
                .GetDealsByFilter($"'LOGIC' => 'OR'," +
                    $"[ 'CONTACT_IDS' => {clientContact}," +
                    $"'CONTACT_ID' => {clientContact} ]," +
                    $"'COMPANY_ID' => {assignedUsers!.First().CompanyId}," +
                    $"'COMPANY_IDS => {assignedUsers!.First().CompanyIds}'" +
                    "'CLOSED' => 'N'");
            //Ишем лида клиента
            var leadsForClient = _repo.Lead
                .GetLeadsByFilter($"'LOGIC' => 'OR'," +
                        $"[ 'CONTACT_IDS' => {clientContact!.First().Id}," +
                        $"'CONTACT_ID' => {clientContact!.First().Id} ]");
            //Ищем связанные с компанией ответственного номера
            var companyContacts = _repo.Contact
                .GetContactsByFilter($"'COMPANY_ID => {assignedUsers!.First().CompanyId}'");
            //На основании статуса звонка выбираем алгоритм
            switch (eventInfo.Stage)
            {
                case "alerting": //Активный дозвон
                    //Ищем по номеру extension группу, в которую входит пользователь
                    var userGroup = _repo.Group.GetGroup(eventInfo.Extension);
                    //Если это отдел продаж и у клиента нет ни лида ни сделки, создаем нового лида
                    if ((userGroup is not null) && 
                        (userGroup.Total > 0) &&
                        (String.Equals(userGroup.Result!.First().Name, "Отдел продаж", StringComparison.InvariantCultureIgnoreCase)) &&
                        ((leadsForClient is not null) && (leadsForClient.IsSuccesful) && (leadsForClient.Total == 0)))
                    {
                        if ((deals is not null) && (deals.IsSuccesful) && (deals.Total == 0))
                        {
                            var newLead = new LeadForCreationDto
                            {
                                Phone = [ new PhoneDto {
                                    Value = eventInfo.Phone,
                                    ValueType = "WORK"
                                } ],
                                AssignedById = assignedUsers!.First().Id
                            };

                            var createdLeadResponse = _repo.Lead.CreateLead(newLead);
                            if ((createdLeadResponse is not null) &&
                                (createdLeadResponse.HasResult))
                            {
                                var newCall = new DealForCallDto
                                {
                                    UserPhoneInner = eventInfo.Direction,
                                    LineNumber = eventInfo.Phone!,
                                    PhoneNumber = eventInfo.Client!,
                                    Type = eventInfo.Direction == "in" ? 1 : 2
                                };

                                //Регистрируем звонок с созданным лидом
                                var registredCall = _repo.Telephony
                                    .RegisterCall(newCall);
                            }
                        }
                        else
                        {
                            var newCall = new DealForCallDto
                            {
                                UserPhoneInner = eventInfo.Direction,
                                LineNumber = eventInfo.Phone!,
                                PhoneNumber = eventInfo.Client!,
                                Type = eventInfo.Direction == "in" ? 1 : 2
                            };

                            //Регистрируем звонок с созданным лидом
                            var registredCall = _repo.Telephony
                                .RegisterCall(newCall);
                        }
                    }
                    //Если звонок поступил не в отдел продаж,
                    //показываем вызов всем пользователям (т.е. пропускаем)
                    _repo.Telephony
                        .ShowCall(eventInfo.Id.ToString(), companyContacts!.Result!.Select(c => c.Id).ToArray());
                    break;
                case "talking": //Активный разговор
                    //В сделке меняем ID ответственного за сделку на ID ответившего на звонок
                    var dealId = deals!.Result!.First().Id;
                    var dealUpdated = _repo.Deal
                        .UpdateDeal(dealId, $"'ASSIGNED_BY_ID' => {assignedUsers!.First().Id}");
                    break;
                case "released": //Звонок завершен
                    //Скрываем карточку контакта
                    _repo.Telephony
                        .HideCall(eventInfo.Id.ToString(), companyContacts!.Result!.Select(c => c.Id).ToArray());
                    break;
            }
        }

        public async Task HandleHistoryRecord(CallRecordDto callRecord)
        {
            //Если вызов входящий
            if (callRecord.Direction == "in") 
            {
                //Создаем сделку из информации о звонке
                var newDeal = new DealForCallDto
                {
                    UserPhoneInner = callRecord.Extension,
                    PhoneNumber = callRecord.Client!,
                    LineNumber = callRecord.Phone!,
                    Show = 0
                };

                //Пробуем зарегистрировать звонок
                _repo.Telephony.RegisterCall(newDeal);
                //Ищем Id сотрудника ответившего на звонок
                var userId = _repo.Contact
                    .GetContactsByFilter($"'PHONE' => {callRecord.Phone}")!.Result!.First().Id;
                //Создаем запись о звонке
                var callInfo = new CallInfoDto
                {
                    CallId = callRecord.Id,
                    UserId = userId,
                    Duration = callRecord.Duration,
                    StatusCode = callRecord.Result,
                    RecordUrl = callRecord.Record,
                    AddToChat = 0
                };

                //Завершаем звонок
                _repo.Telephony.FinishCall(callInfo);

                //Добавляем запись в карточку
                _repo.Telephony.AttachRecord(callRecord.Id, callRecord.Record!);
            }
            //Если звонок не входяший - игнорируем
        }
    }
}
