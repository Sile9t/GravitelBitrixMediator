using Entities.Dtos.Gravitel;

namespace Services.Contracts
{
    public interface ICallService
    {
        Task<(string companyTitle, string userPhoneInner)>
            GetCompanyTitleAndUserPhoneInnerForClientDeal(GravitelClientCallInfoDto callInfo);
    }
}
