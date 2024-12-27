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

        public void HandleEvent(EventInfoDto eventInfo)
        {
            //На основании статуса звонка выбираем алгоритм
            switch (eventInfo.Stage)
            {
                case "alerting": //Активный дозвон
                    //TODO
                    //Ищем по номеру extension группу, в которую входит пользователь
                    var userGroup = _repo.;

                    //Если это отдел продаж и у клиента нет ни лида ни сделки, создаем нового лида
                    if (String.Compare(userGroup.Name, "Отдел продаж",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        var newLead = new LeadForCreationDto
                        {
                            Phone = [ new PhoneDto {
                                Value = eventInfo.Phone,
                                ValueType = "WORK"
                            } ],
                            AssignedById = _assignedUserIdsList.First()
                        };
                        var createdLeadResponse = CreateLead(newLead);
                        if ((createdLeadResponse is not null) &&
                            (createdLeadResponse.HasResult))
                        {
                            long createdLeadId = createdLeadResponse.Result;
                            var newDealForCall = new DealForCallDto
                            {
                                UserPhoneInner = eventInfo.Direction,
                                LineNumber = eventInfo.Phone!,
                                PhoneNumber = eventInfo.Client!,
                                Type = eventInfo.Direction == "in" ? 1 : 2
                            };

                            //Регистрируем звонок с созданным лидом
                            var registredCallDeal = RegisterCall(newDealForCall);
                        }
                    }
                    //Если звонок поступил не в отдел продаж показываем вызов всем пользователям

                    break;
                case "talking": //Активный разговор
                    //Ищем ID ответившего
                    //В сделке меняет ID ответственного за сделку на ID ответившего на звонок
                    break;
                case "released": //Звонок завершен
                    //Скрываем карточку контакта
                    break;
                default: //В любом другом случае
                    break;
            }
        }
    }
}
