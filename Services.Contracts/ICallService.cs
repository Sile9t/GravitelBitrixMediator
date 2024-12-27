using Entities.Dtos.Gravitel;

namespace Services.Contracts
{
    public interface ICallService
    {
        (string companyTitle, string userPhoneInner)
            GetCompanyTitleAndUserPhoneInnerForClientDeal(GravitelClientCallInfoDto callInfo);
    }
}
