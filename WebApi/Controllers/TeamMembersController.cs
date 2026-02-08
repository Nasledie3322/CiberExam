using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Application.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamMembersController : ControllerBase
{
    private readonly ITeamMemberService _teamMemberService;

    public TeamMembersController(ITeamMemberService teamMemberService)
    {
        _teamMemberService = teamMemberService;
    }

    [HttpGet]
    public async Task<Response<List<TeamMember>>> GetTeamMembers()
    {
        return await _teamMemberService.GetTeamMembers();
    }

    [HttpGet("{teamId}/{userId}")]
    public async Task<Response<TeamMember>> GetTeamMemberById(long teamId, long userId)
    {
        return await _teamMemberService.GetTeamMemberById(teamId, userId);
    }

    [HttpPost]
    public async Task<Response<string>> AddTeamMember(AddTeamMemberDto dto)
    {
        return await _teamMemberService.Add(dto);
    }

    [HttpDelete("{teamId}/{userId}")]
    public async Task<Response<string>> DeleteTeamMember(long teamId, long userId)
    {
        return await _teamMemberService.Delete(teamId, userId);
    }
}
