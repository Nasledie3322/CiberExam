using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Application.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchResultsController : ControllerBase
{
    private readonly IMatchResultService _matchResultService;

    public MatchResultsController(IMatchResultService matchResultService)
    {
        _matchResultService = matchResultService;
    }

    [HttpGet]
    public async Task<Response<List<MatchResult>>> GetMatchResults()
    {
        return await _matchResultService.GetMatchResults();
    }

    [HttpGet("{matchId}/{teamId}")]
    public async Task<Response<MatchResult>> GetMatchResultById(long matchId, long teamId)
    {
        return await _matchResultService.GetMatchResultById(matchId, teamId);
    }

    [HttpPost]
    public async Task<Response<string>> AddMatchResult(AddMatchResultDto dto)
    {
        return await _matchResultService.Add(dto);
    }

    [HttpPut("{matchId}/{teamId}")]
    public async Task<Response<string>> UpdateMatchResult(long matchId, long teamId, UpdateMatchResultDto dto)
    {
        return await _matchResultService.Update(matchId, teamId, dto);
    }

    [HttpDelete("{matchId}/{teamId}")]
    public async Task<Response<string>> DeleteMatchResult(long matchId, long teamId)
    {
        return await _matchResultService.Delete(matchId, teamId);
    }
}
