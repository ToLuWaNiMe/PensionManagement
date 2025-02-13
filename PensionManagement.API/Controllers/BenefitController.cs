using Microsoft.AspNetCore.Mvc;
using PensionManagement.Application.DTOs;
using PensionManagement.Application.Services;
using PensionManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PensionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BenefitController : ControllerBase
    {
        private readonly BenefitService _benefitService;

        public BenefitController(BenefitService benefitService)
        {
            _benefitService = benefitService;
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<BenefitDto>>> GetByMemberId(int memberId)
        {
            return Ok(await _benefitService.GetAllByMemberIdAsync(memberId));
        }

        [HttpPost("eligibility")]
        public async Task<IActionResult> CheckEligibility([FromBody] BenefitDto request)
        {
            var isEligible = await _benefitService.IsEligibleForBenefit(request.MemberId, request.BenefitType);
            return Ok(new { isEligible });
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateBenefit([FromBody] BenefitDto request)
        {
            var amount = await _benefitService.CalculateBenefitAmount(request.MemberId, request.BenefitType);
            return Ok(new { amount });
        }
    }
}
