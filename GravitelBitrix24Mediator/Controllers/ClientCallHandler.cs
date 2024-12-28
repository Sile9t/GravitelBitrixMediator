using Entities.Dtos.Gravitel;
using Entities.Exceptions;
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
        
        public ClientCallHandler(IServiceManager service) =>
            _service = service;

        [HttpPost("client")]
        public async Task<IActionResult> Client([FromBody] GravitelClientCallInfoDto? callInfo)
        {
            if (callInfo is null)
                throw new CallInfoBadRequestException();

            if (String.IsNullOrEmpty(callInfo.ClientPhone))
                throw new ClientPhoneBadRequestException();

            var result = _service.CallService.GetCompanyTitleAndUserPhoneInnerForClientDeal(callInfo);

            return Ok(result);
        }

        [HttpPost("event")]
        public async Task<IActionResult> Event([FromBody] EventInfoDto eventInfo)
        {
            if (String.Equals(eventInfo.Direction, "out", StringComparison.CurrentCultureIgnoreCase))
                return Ok(eventInfo);

            await _service.EventService.HandleEvent(eventInfo);

            return Ok(eventInfo);
        }

        [HttpPost("history")]
        public async Task<IActionResult> History([FromBody] CallRecordDto callRecord)
        {
            await _service.EventService.HandleHistoryRecord(callRecord);
            
            return Ok();
        }
    }
}
