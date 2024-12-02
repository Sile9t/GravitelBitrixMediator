﻿using Entities.Dtos.Bitrix;

namespace Repository.Contracts.Repos
{
    public interface ITelephonyRepository : IRepositoryBase
    {
        CRMEntityDto[]? GetCrmEntityByPhone(string phone);
        void ShowCall(string CallId, int[] UserId);
        CallHistory[]? FinishCall(CallInfoDto callInfo);
    }
}