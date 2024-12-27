using Entities.Dtos.Gravitel;

namespace Services.Contracts
{
    public interface IEventService
    {
        void HandleEvent(EventInfoDto eventInfo);
    }
}
