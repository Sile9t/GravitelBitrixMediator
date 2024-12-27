using Entities.Dtos.Bitrix;
using Entities.Dtos.Gravitel;
using Entities.Exceptions;
using Entities.Responses;
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
        private List<CompanyDto> _companyList = new();
        private List<DealDto> _dealList = new();
        private List<LeadDto> _leadList = new();
        private List<long> _assignedUserIdsList = new();

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

            //

            return Ok(eventInfo);
        }

        [HttpPost("history")]
        public async Task<IActionResult> History()
        {
            //TODO: Перед завершением звонка надо проверить вызывалась ли его регистрация

            //Если звонок зарегистрирован, завершаем его

            //Добавляем запись в карточку
            return Ok();
        }
    }
}
