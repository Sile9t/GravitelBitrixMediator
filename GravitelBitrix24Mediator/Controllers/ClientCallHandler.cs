using Entities.Dtos.Bitrix;
using Entities.Dtos.Gravitel;
using Entities.Exceptions;
using Entities.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace GravitelBitrix24Mediator.Controllers
{
    [ApiController]
    //Change to your crm_url
    [Route("crm_url")]
    public class ClientCallHandler : Controller
    {
        private readonly IServiceManager _service;
        private List<CompanyDto> _companyList = new();
        private List<DealDto> _dealList = new();
        private List<LeadDto> _leadList = new();
        private List<long> _assignedUserIdsList = new();

        public ClientCallHandler(IServiceManager service) =>
            _service = service;

        [HttpPost("client")]
        public async Task<IActionResult> Client([FromBody] GravitelClientCallInfoDto? callInfo)
        {
            if (callInfo is null)
                throw new CallInfoBadRequestException();

            if (String.IsNullOrEmpty(callInfo.ClientPhone))
                throw new ClientPhoneBadRequestException();

            //Ищем контакт клиента
            var clientContactInfo = GetCrmEntityByPhone(callInfo.ClientPhone);

            if (String.IsNullOrEmpty(callInfo.Phone))
                throw new UserPhoneBadRequestException();

            //Ищем информацию о клиенте через фильтр контактов по номеру телефона
            var clientsList = GetContactsByFilter($"'PHONE': {callInfo.ClientPhone}");

            if ((clientsList is not null) &&
                (clientsList.Total > 0))
            {
                //Берем ID первого контакта
                long firstClientId = clientsList.Result![0].Id;

                //Ищем компании привязанные к номеру пользователя
                var companiesForUser = GetCompaniesByFilter($"'PHONE': {callInfo.Phone}");

                //Инициализируем переменные названия компании и внуртеннего номера ответственного
                string finalCompanyTitle = "";
                string finalUserPhoneInner = "";

                //Проходим по списку компаний и ищем у них октрытые на клиента сделки
                if ((companiesForUser is not null) && (companiesForUser.Total > 0))
                {
                    foreach (var company in companiesForUser.Result!)
                    {
                        var deals = GetDealsByFilter($"'LOGIC' => 'OR'," +
					    $"[ 'CONTACT_IDS' => {clientContactInfo}," +
						    $"'CONTACT_ID' => {firstClientId} ]," +
					    $"'COMPANY_ID' => {company.Id}," +
					    "'CLOSED' => 'N'");

                        if ((deals is not null) && (deals.Total > 0))
                        {
                            //Если сделок несколько добавляем компанию в локальный список
                            _companyList.Add(company);

                            //Проходим по всем сделкам и записываем ID ответсвенных за них в локальный список
                            foreach(var deal in deals.Result!)
                            {
                                _dealList.Add(deal);

                                //Если у сделки есть ответственнный добавляем его в локальный список
                                if (deal.AssignedById is not null)
                                {
                                    _assignedUserIdsList.Add((long)deal.AssignedById);
                                    if (String.IsNullOrEmpty(finalCompanyTitle))
                                        finalCompanyTitle = company.Title!;
                                }
                            }
                        }
                    }

                    //Ищем номер первого ответственного пользователя
                    var assignedUserContact = GetContactsByFilter($"'ID': {_assignedUserIdsList.First()}");

                    if ((assignedUserContact is not null) && 
                        (assignedUserContact.Total > 0) &&
                        (assignedUserContact.Result![0].Phone != Array.Empty<PhoneDto>()))
                    {
                        var assignedUserPhone = assignedUserContact.Result[0].Phone?[0].Value;

                        //Ищем контакт ответственного пользователя
                        var assignedUserEntity = GetCrmEntityByPhone(assignedUserPhone!);

                        if ((assignedUserEntity is not null) &&
                            (assignedUserEntity.Length > 0))
                            finalUserPhoneInner = assignedUserEntity[0].AssignedBy[0].UserPhoneInner;
                    }
                }
                else
                {
                    //Если сделок нету, ищем лида по ID клиента
                    var leadsForClient = GetLeadsByFilter($"'CONTACT_ID': {firstClientId}");
                    if ((leadsForClient is not null) &&
                        (leadsForClient.Total > 0))
                    {
                        _leadList.AddRange(leadsForClient.Result!);
                        //Если лидов несколько, берем контакт ответственного за первый лид
                        var firstLead = leadsForClient.Result![0];

                        if (firstLead.Phone != Array.Empty<PhoneDto>()) 
                        {
                            var assignedUserEntity = GetCrmEntityByPhone(firstLead.Phone?[0].Value!);

                            finalCompanyTitle = firstLead.CompanyTitle!;
                            if ((assignedUserEntity is not null) && 
                                (assignedUserEntity != Array.Empty<CRMEntityDto>()) &&
                                (assignedUserEntity[0].AssignedBy != Array.Empty<AssignedEntityDto>()))
                                finalUserPhoneInner = assignedUserEntity[0].AssignedBy[0].UserPhoneInner;
                        }
                    }
                }

                if (String.IsNullOrEmpty(finalCompanyTitle) &&
                    (String.IsNullOrEmpty(finalUserPhoneInner)))
                {
                    //Если значение названия и внутренний номер пользователя не пусты,
                    //возврашаем их как ответ от платформы
                    return Ok((finalCompanyTitle, finalUserPhoneInner));
                }
            }

            //Если алгоритм ничего не нашел, возвращаем входные данные
            return Ok(callInfo);
        }

        [HttpPost("event")]
        public async Task<IActionResult> Event([FromBody] EventInfoDto eventInfo)
        {
            if (String.Equals(eventInfo.Direction, "out", StringComparison.CurrentCultureIgnoreCase))
                return Ok(eventInfo);

            //На основании статуса звонка выбираем алгоритм
            switch (eventInfo.Stage)
            {
                case "alerting": //Активный дозвон
                    //TODO
                    //Ищем по номеру extension группу, в которую входит пользователь
                    var userGroup;

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
                                Type = eventInfo.Direction == "in"? 1 : 2
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

            //

            return Ok(eventInfo);
        }

        [HttpPost("history")]
        public async Task<IActionResult> History()
        {
            //TODO: Перед завершением звонка надо проверить вызывалась ли его регистрация

            //Если звонок зарегистрирован, завершаем его

            //Добавляем запись в карточку
            return Ok();
        }

        private CRMEntityDto[]? GetCrmEntityByPhone(string phone)
        {
            string response = _bitrix.SendCommand("telephony.externalCall.searchCrmEntities",
                $"PHONE_NUMBER={phone}");

            return Deserialize<CRMEntityDto[]>(response);
        }

        private RegistredDealForCallDto? RegisterCall(DealForCallDto dealInfo)
        {
            string body = Serialize(dealInfo);
            string response = _bitrix.SendCommand("telephony.externalCall.register",
                Body: body);

            return Deserialize<RegistredDealForCallDto>(response);
        }

        private void ShowCall(string CallId, int UserId)
        {
            ShowCall(CallId, [ UserId ]);
        }

        private void ShowCall(string CallId, int[] UserId)
        {
            string response = _bitrix.SendCommand("telephony.externalCall.show",
                $" CALL_ID:{CallId}, USER_ID: {UserId}");
        }

        private CallHistory[]? FinishCall(CallInfoDto callInfo)
        {
            string body = Serialize(callInfo);
            string response = _bitrix.SendCommand("telephony.exteranalCall.finish",
                Body: body);

            return Deserialize<CallHistory[]>(response);
        }

        private Response<CompanyDto>? GetCompany(string id)
        {
            string response = _bitrix.SendCommand("crm.company.get",
                $" ID: {id}");

            return Deserialize<Response<CompanyDto>>(response);
        }

        private ListResponse<CompanyDto>? GetCompaniesByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.company.list",
                "SELECT: [ 'ID', 'TITLE', 'PHONE' ]," +
                "FILTER: [ " + filter + " ]"
                );

            return Deserialize<ListResponse<CompanyDto>>(response);
        }

        private Response<long>? CreateLead(LeadForCreationDto leadForCreation)
        {
            string body = Serialize(leadForCreation);
            string response = _bitrix.SendCommand("crm.lead.add",
                Body: body);

            return Deserialize<Response<long>>(response);
        }

        private ListResponse<LeadDto>? GetLeadsByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.lead.list",
                "SELECT: [ 'ID', 'COMPANY_ID', 'COMPANY_TITLE', 'CONTACT_ID', 'ASSIGNED_BY_ID', 'PHONE' ]," +
                "FILTER: [ " + filter + " ]"
                );

            return Deserialize<ListResponse<LeadDto>>(response);
        }

        private ListResponse<DealDto>? GetDealsByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.deal.list",
                "SELECT: [ 'ID', 'ASSIGNED_BY_ID', 'ASSIGNED_BY', 'LEAD_ID', 'LEAD_BY', 'COMPANY_ID'," 
                    + "'CONTACT_ID', 'CONTACT_IDS' ]," +
                "FILTER: [ " + filter + " ]"
                );

            return Deserialize<ListResponse<DealDto>>(response);
        }

        private ListResponse<ContactDto>? GetContactsByFilter(string filter)
        {
            string response = _bitrix.SendCommand("crm.contact.list",
                "SELECT: [ 'ID', 'NAME', 'PHONE' ]," + 
                "FILTER: [ " + filter + "]"
                );

            return Deserialize<ListResponse<ContactDto>>(response);
        }

        private T? Deserialize<T>(string data)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(data);
        }

        private string Serialize<T>(T data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data);
        }
    }
}
