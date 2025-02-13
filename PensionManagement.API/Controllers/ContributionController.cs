using Microsoft.AspNetCore.Mvc;
using PensionManagement.Application.DTOs;
using PensionManagement.Application.Services;

namespace PensionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContributionController : ControllerBase
    {
        private readonly ContributionService _contributionService;

        public ContributionController(ContributionService contributionService)
        {
            _contributionService = contributionService;
        }

        [HttpGet("member/{memberId}")]
        public async Task<ActionResult<IEnumerable<ContributionDto>>> GetByMemberId(int memberId)
        {
            return Ok(await _contributionService.GetAllByMemberIdAsync(memberId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContributionDto contributionDto)
        {
            await _contributionService.AddAsync(contributionDto);
            return CreatedAtAction(nameof(GetByMemberId), new { memberId = contributionDto.MemberId }, contributionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContributionDto contributionDto)
        {
            await _contributionService.UpdateAsync(id, contributionDto);
            return NoContent();
        }

        [HttpGet("member/{memberId}/total")]
        public async Task<ActionResult<decimal>> GetTotalContributions(int memberId)
        {
            var contributions = await _contributionService.GetAllByMemberIdAsync(memberId);
            var total = contributions.Sum(c => c.Amount);
            return Ok(total);
        }
    }
}
