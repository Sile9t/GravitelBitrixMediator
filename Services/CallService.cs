using Contracts;
using Entities.Dtos.Bitrix;
using Entities.Dtos.Gravitel;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class CallService : ICallService
    {
        private readonly ILoggerManager _logger;
        private readonly IBitrixRepositoryManager _bitrix;
        private readonly IGravitelRepositoryManager _gravitel;
        private List<CompanyDto> _companyList = new();
        private List<DealDto> _dealList = new();
        private List<LeadDto> _leadList = new();
        private List<long> _assignedUserIdsList = new();


        public CallService(IBitrixRepositoryManager bitrix, IGravitelRepositoryManager gravitel, ILoggerManager logger)
        {
            _logger = logger;
            _bitrix = bitrix;
            _gravitel = gravitel;
        }

        public (string companyTitle, string userPhoneInner) 
            GetCompanyTitleAndUserPhoneInnerForClientDeal(GravitelClientCallInfoDto callInfo)
        {
            var clientContactInfo = _bitrix.Telephony
                .GetCrmEntityByPhone(callInfo.ClientPhone);

            //Ищем информацию о клиенте через фильтр контактов по номеру телефона
            var clientsList = _bitrix.Contact
                .GetContactsByFilter(filter: $"'PHONE': {callInfo.ClientPhone}");

            if ((clientsList is not null) &&
                (clientsList.Total > 0))
            {
                //Берем ID первого контакта
                long firstClientId = clientsList.Result![0].Id;

                //Ищем компании привязанные к номеру пользователя
                var companiesForUser = _bitrix.Company
                    .GetCompaniesByFilter($"'PHONE': {callInfo.Phone}");

                //Инициализируем переменные названия компании и внуртеннего номера ответственного
                string finalCompanyTitle = "";
                string finalUserPhoneInner = "";

                //Проходим по списку компаний и ищем у них октрытые на клиента сделки
                if ((companiesForUser is not null) && (companiesForUser.Total > 0))
                {
                    foreach (var company in companiesForUser.Result!)
                    {
                        var deals = _bitrix.Deal
                            .GetDealsByFilter($"'LOGIC' => 'OR'," +
                                $"[ 'CONTACT_IDS' => {clientContactInfo}," +
                                $"'CONTACT_ID' => {firstClientId} ]," +
                                $"'COMPANY_ID' => {company.Id}," +
                                "'CLOSED' => 'N'");

                        if ((deals is not null) && (deals.Total > 0))
                        {
                            //Если сделок несколько добавляем компанию в локальный список
                            _companyList.Add(company);

                            //Проходим по всем сделкам и записываем ID ответсвенных за них в локальный список
                            foreach (var deal in deals.Result!)
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
                    var assignedUserContact = _bitrix.Contact
                        .GetContactsByFilter($"'ID': {_assignedUserIdsList.First()}");

                    if ((assignedUserContact is not null) &&
                        (assignedUserContact.Total > 0) &&
                        (assignedUserContact.Result![0].Phone != Array.Empty<PhoneDto>()))
                    {
                        var assignedUserPhone = assignedUserContact.Result[0].Phone?[0].Value;

                        //Ищем контакт ответственного пользователя
                        var assignedUserEntity = _bitrix.Telephony
                            .GetCrmEntityByPhone(assignedUserPhone!);

                        if ((assignedUserEntity is not null) &&
                            (assignedUserEntity.Length > 0))
                            finalUserPhoneInner = assignedUserEntity[0].AssignedBy[0].UserPhoneInner;
                    }
                }
                else
                {
                    //Если сделок нету, ищем лида по ID клиента
                    var leadsForClient = _bitrix.Lead
                        .GetLeadsByFilter($"'LOGIC' => 'OR'," +
                                $"[ 'CONTACT_IDS' => {firstClientId}," +
                                $"'CONTACT_ID' => {firstClientId} ]");
                    if ((leadsForClient is not null) &&
                        (leadsForClient.Total > 0))
                    {
                        _leadList.AddRange(leadsForClient.Result!);
                        //Если лидов несколько, берем контакт ответственного за первый лид
                        var firstLead = leadsForClient.Result![0];

                        if (firstLead.Phone != Array.Empty<PhoneDto>())
                        {
                            var assignedUserEntity = _bitrix.Telephony
                                .GetCrmEntityByPhone(firstLead.Phone?[0].Value!);

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
                    return (finalCompanyTitle, finalUserPhoneInner);
                }
            }

            return ("", "");
        }
    }
}
