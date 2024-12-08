﻿using Entities.Dtos.Bitrix;

namespace Repository.Contracts.Repos
{
    public interface ITelephonyRepository : IRepositoryBase
    {
        CRMEntityDto[]? GetCrmEntityByPhone(string phone);
        RegistredDealForCallDto? RegisterCall(DealForCallDto dealInfo);
        void ShowCall(string CallId, int[] UserId);
        CallHistory[]? FinishCall(CallInfoDto callInfo);
    }
}
