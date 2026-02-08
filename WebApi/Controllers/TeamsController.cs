using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Application.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<Response<List<Team>>> GetTeams()
    {
        return await _teamService.GetTeams();
    }

    [HttpGet("{id}")]
    public async Task<Response<Team>> GetTeamById(long id)
    {
        return await _teamService.GetTeamById(id);
    }

    [HttpPost]
    public async Task<Response<string>> AddTeam(AddTeamDto dto)
    {
        return await _teamService.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateTeam(long id, UpdateTeamDto dto)
    {
        return await _teamService.Update(id, dto);
    }

    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteTeam(long id)
    {
        return await _teamService.Delete(id);
    }
}
