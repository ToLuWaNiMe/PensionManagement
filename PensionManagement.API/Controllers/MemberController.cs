using Microsoft.AspNetCore.Mvc;
using PensionManagement.Application.DTOs;
using PensionManagement.Application.Services;

namespace PensionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MemberController(MemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("members/{Total}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
        {
            return Ok(await _memberService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDto>> GetById(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            return member == null ? NotFound() : Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MemberDto memberDto)
        {
            await _memberService.AddAsync(memberDto);
            return CreatedAtAction(nameof(GetById), new { id = memberDto.Email }, memberDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MemberDto memberDto)
        {
            await _memberService.UpdateAsync(id, memberDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memberService.DeleteAsync(id);
            return NoContent();
        }
    }
}
