﻿using Contracts;
using Entities.Dtos.Bitrix;
using Entities.Dtos.Gravitel;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class EventService : IEventService
    {
        IBitrixServiceManager _bitrix;
        IGravitelServiceManager _gravitel;
        ILoggerManager _logger;

        public EventService(IBitrixServiceManager bitrix, IGravitelServiceManager gravitel, ILoggerManager logger)
        {
            _bitrix = bitrix;
            _logger = logger;
            _gravitel = gravitel;
        }

        public async Task HandleEvent(EventInfoDto eventInfo)
        {
            //Ищем Id пользователя
            var assignedUsers = await _bitrix.Contact
                .GetContactsByFilter($"'PHONE' => {eventInfo.Phone}");
            var clientContact = await _bitrix.Contact
                .GetContactsByFilter($"'PHONE' => {eventInfo.Client}");
            //Ищем сделки клиента
            var deals = await _bitrix.Deal
                .GetDealsByFilter($"'LOGIC' => 'OR'," +
                    $"[ 'CONTACT_IDS' => {clientContact}," +
                    $"'CONTACT_ID' => {clientContact} ]," +
                    $"'COMPANY_ID' => {assignedUsers!.First().CompanyId}," +
                    $"'COMPANY_IDS => {assignedUsers!.First().CompanyIds}'" +
                    "'CLOSED' => 'N'");
            //Ишем лида клиента
            var leadsForClient = await _bitrix.Lead
                .GetLeadsByFilter($"'LOGIC' => 'OR'," +
                        $"[ 'CONTACT_IDS' => {clientContact!.First().Id}," +
                        $" 'CONTACT_ID' => {clientContact!.First().Id} ]");
            //Ищем связанные с компанией ответственного номера
            var companyContacts = await _bitrix.Contact
                .GetContactsByFilter($"'COMPANY_ID => {assignedUsers!.First().CompanyId}'");
            //На основании статуса звонка выбираем алгоритм
            switch (eventInfo.Stage)
            {
                case "alerting": //Активный дозвон
                    //Ищем по номеру extension группу, в которую входит пользователь
                    var userGroups = await _bitrix.Group.GetGroupsByFilter($"ID => {eventInfo.Extension}");
                    var gravitelGroups = await _gravitel.Group.GetGroups();
                    var gravitelGroup = gravitelGroups.FirstOrDefault(g => g.Extension == eventInfo.Extension.ToString() && g.Name!.Equals("Отдел продаж", StringComparison.InvariantCultureIgnoreCase));
                    //Если это отдел продаж и у клиента нет ни лида ни сделки, создаем нового лида
                    if (userGroups.Any(g => string.Equals(g.Name, "Отдел продаж", StringComparison.InvariantCultureIgnoreCase)) &&
                        (gravitelGroup is not null) &&
                        (leadsForClient.Count() > 0))
                    {
                        if ((deals.Count() > 0))
                        {
                            var newLead = new LeadForCreationDto
                            {
                                Phone = [ new PhoneDto {
                                    Value = eventInfo.Phone,
                                    ValueType = "WORK"
                                } ],
                                AssignedById = assignedUsers!.First().Id
                            };

                            var createdLeadResponse = await _bitrix.Lead.CreateLead(newLead);
                        }

                        var newCall = new DealForCallDto
                        {
                            UserPhoneInner = eventInfo.Direction,
                            LineNumber = eventInfo.Phone!,
                            PhoneNumber = eventInfo.Client!,
                            Type = eventInfo.Direction == "in" ? 1 : 2
                        };

                        //Регистрируем звонок
                        var registredCall = _bitrix.Telephony
                            .RegisterCall(newCall);
                    }
                    //Если звонок поступил не в отдел продаж,
                    //показываем вызов всем пользователям (т.е. пропускаем)
                    await _bitrix.Telephony
                        .ShowCall(eventInfo.Id.ToString(), companyContacts!.Select(c => c.Id).ToArray());
                    break;
                case "talking": //Активный разговор
                    //В сделке меняем ID ответственного за сделку на ID ответившего на звонок
                    var dealId = deals!.First().Id;
                    var dealUpdated = _bitrix.Deal
                        .UpdateDeal(dealId, $"'ASSIGNED_BY_ID' => {assignedUsers!.First().Id}");
                    break;
                case "released": //Звонок завершен
                    //Скрываем карточку контакта
                    await _bitrix.Telephony
                        .HideCall(eventInfo.Id.ToString(), companyContacts!.Select(c => c.Id).ToArray());
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
                await _bitrix.Telephony.RegisterCall(newDeal);
                //Ищем Id сотрудника ответившего на звонок
                var userId = _bitrix.Contact
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
                await _bitrix.Telephony.FinishCall(callInfo);

                //Добавляем запись в карточку
                await _bitrix.Telephony.AttachRecord(callRecord.Id, callRecord.Record!);
            }
            //Если звонок не входяший - игнорируем
        }
    }
}
