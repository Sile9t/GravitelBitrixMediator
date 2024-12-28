using Entities.Dtos.Gravitel;

namespace Services.Contracts
{
    public interface IEventService
    {
        Task HandleEvent(EventInfoDto eventInfo);
        Task HandleHistoryRecord(CallRecordDto callRecord);
    }
}
