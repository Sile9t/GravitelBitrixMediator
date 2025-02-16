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
        private readonly IBitrixServiceManager _bitrix;
        private readonly IGravitelServiceManager _gravitel;
        private List<CompanyDto> _companyList = new();
        private List<DealDto> _dealList = new();
        private List<LeadDto> _leadList = new();
        private List<long> _assignedUserIdsList = new();


        public CallService(IBitrixServiceManager bitrix, IGravitelServiceManager gravitel, ILoggerManager logger)
        {
            _logger = logger;
            _bitrix = bitrix;
            _gravitel = gravitel;
        }

        public async Task<(string companyTitle, string userPhoneInner)> 
            GetCompanyTitleAndUserPhoneInnerForClientDeal(GravitelClientCallInfoDto callInfo)
        {
            var clientContactInfo = await _bitrix.Telephony
                .GetCrmEntityByPhone(callInfo.ClientPhone);

            //Ищем информацию о клиенте через фильтр контактов по номеру телефона
            var clientsList = await _bitrix.Contact
                .GetContactsByFilter(filter: $"'PHONE': {callInfo.ClientPhone}");

            if (clientsList.Any())
            {
                //Берем ID первого контакта
                long firstClientId = clientsList.First().Id;

                //Ищем компании привязанные к номеру пользователя
                var companiesForUser = await _bitrix.Company
                    .GetCompaniesByFilter($"'PHONE': {callInfo.Phone}");

                //Инициализируем переменные названия компании и внуртеннего номера ответственного
                string finalCompanyTitle = "";
                string finalUserPhoneInner = "";

                //Проходим по списку компаний и ищем у них октрытые на клиента сделки
                if (companiesForUser.Any())
                {
                    foreach (var company in companiesForUser)
                    {
                        var deals = await _bitrix.Deal
                            .GetDealsByFilter($"'LOGIC' => 'OR'," +
                                $"[ 'CONTACT_IDS' => {clientContactInfo}," +
                                $"'CONTACT_ID' => {firstClientId} ]," +
                                $"'COMPANY_ID' => {company.Id}," +
                                "'CLOSED' => 'N'");

                        if (deals.Count() > 0)
                        {
                            //Если сделок несколько добавляем компанию в локальный список
                            _companyList.Add(company);

                            //Проходим по всем сделкам и записываем ID ответсвенных за них в локальный список
                            foreach (var deal in deals)
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
                    var assignedUserContact = await _bitrix.Contact
                        .GetContactsByFilter($"'ID': {_assignedUserIdsList.First()}");

                    if ((assignedUserContact.Any()) &&
                        (assignedUserContact.First().Phone.Count() > 0))
                    {
                        var assignedUserPhone = assignedUserContact.First().Phone[0].Value;

                        //Ищем контакт ответственного пользователя
                        var assignedUserEntity = await _bitrix.Telephony
                            .GetCrmEntityByPhone(assignedUserPhone!);

                        if (assignedUserEntity.Any())
                            finalUserPhoneInner = assignedUserEntity.First().AssignedBy[0].UserPhoneInner;
                    }
                }
                else
                {
                    //Если сделок нету, ищем лида по ID клиента
                    var leadsForClient = await _bitrix.Lead
                        .GetLeadsByFilter($"'LOGIC' => 'OR'," +
                                $"[ 'CONTACT_IDS' => {firstClientId}," +
                                $"'CONTACT_ID' => {firstClientId} ]");
                    if (leadsForClient.Any())
                    {
                        _leadList.AddRange(leadsForClient);
                        //Если лидов несколько, берем контакт ответственного за первый лид
                        var firstLead = leadsForClient.First();

                        if (firstLead.Phone.Length > 0)
                        {
                            var assignedUserEntity = await _bitrix.Telephony
                                .GetCrmEntityByPhone(firstLead.Phone?[0].Value!);

                            finalCompanyTitle = firstLead.CompanyTitle!;
                            if (assignedUserEntity.First().AssignedBy.Any())
                                finalUserPhoneInner = assignedUserEntity.First().AssignedBy.First().UserPhoneInner;
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
