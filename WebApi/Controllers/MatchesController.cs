using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Application.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchesController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpGet]
    public async Task<Response<List<Match>>> GetMatches()
    {
        return await _matchService.GetMatches();
    }

    [HttpGet("{id}")]
    public async Task<Response<Match>> GetMatchById(long id)
    {
        return await _matchService.GetMatchById(id);
    }

    [HttpPost]
    public async Task<Response<string>> AddMatch(AddMatchDto dto)
    {
        return await _matchService.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateMatch(long id, UpdateMatchDto dto)
    {
        return await _matchService.Update(id, dto);
    }

    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteMatch(long id)
    {
        return await _matchService.Delete(id);
    }
}
