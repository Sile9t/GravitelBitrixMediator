using Entities;
using Entities.Dtos.Bitrix;
using Entities.Responses;
using RestSharp;
using Services.Contracts;

namespace Services
{
    public static class BitrixExtentions
    {
        public static CRMEntityDto[]? GetCrmEntityByPhone(this IBitrix client, string phone)
        {
            string response = client.SendCommand("telephony.externalCall.searchCrmEntities",
                $"PHONE_NUMBER={phone}");

            return Deserialize<CRMEntityDto[]>(response);
        }

        public static Response<CompanyDto>? GetCompany(this IBitrix client, string id)
        {
            string response = client.SendCommand("crm.company.get",
                $" ID: {id}");

            return Deserialize<Response<CompanyDto>>(response);
        }

        public static ListResponse<CompanyDto>? GetCompaniesByFilter(this IBitrix client, string filter)
        {
            string response = client.SendCommand("crm.company.list",
                "SELECT: [ 'ID', 'TITLE', 'PHONE' ]," +
            "FILTER: [ " + filter + " ]"
            );

            return Deserialize<ListResponse<CompanyDto>>(response);
        }

        public static ListResponse<LeadDto>? GetLeadsByFilter(this IBitrix client, string filter)
        {
            string response = client.SendCommand("crm.lead.list",
                "SELECT: [ 'ID', 'COMPANY_ID', 'COMPANY_TITLE', 'CONTACT_ID', 'ASSIGNED_BY_ID', 'PHONE' ]," +
            "FILTER: [ " + filter + " ]"
            );

            return Deserialize<ListResponse<LeadDto>>(response);
        }

        public static ListResponse<DealDto>? GetDealsByFilter(this IBitrix client, string filter)
        {
            string response = client.SendCommand("crm.deal.list",
                "SELECT: [ 'ID', 'ASSIGNED_BY_ID', 'ASSIGNED_BY', 'LEAD_ID', 'LEAD_BY', 'COMPANY_ID',"
                    + "'CONTACT_ID', 'CONTACT_IDS' ]," +
                "FILTER: [ " + filter + " ]"
            );

            return Deserialize<ListResponse<DealDto>>(response);
        }

        public static ListResponse<ContactDto>? GetContactsByFilter(this IBitrix client, string filter)
        {
            string response = client.SendCommand("crm.contact.list",
                "SELECT: [ 'ID', 'NAME', 'PHONE' ]," +
                "FILTER: [ " + filter + "]"
            );

            return Deserialize<ListResponse<ContactDto>>(response);
        }

        private static T? Deserialize<T>(string data)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(data);
        }
    }
}
