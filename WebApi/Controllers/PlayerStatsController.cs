using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Application.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerStatsController : ControllerBase
{
    private readonly IPlayerStatsService _playerStatsService;

    public PlayerStatsController(IPlayerStatsService playerStatsService)
    {
        _playerStatsService = playerStatsService;
    }

    [HttpGet]
    public async Task<Response<List<PlayerStats>>> GetPlayerStats()
    {
        return await _playerStatsService.GetPlayerStats();
    }

    [HttpGet("{matchId}/{userId}")]
    public async Task<Response<PlayerStats>> GetPlayerStatsById(long matchId, long userId)
    {
        return await _playerStatsService.GetPlayerStatsById(matchId, userId);
    }

    [HttpPost]
    public async Task<Response<string>> AddPlayerStats(AddPlayerStatsDto dto)
    {
        return await _playerStatsService.Add(dto);
    }

    [HttpPut("{matchId}/{userId}")]
    public async Task<Response<string>> UpdatePlayerStats(long matchId, long userId, UpdatePlayerStatsDto dto)
    {
        return await _playerStatsService.Update(matchId, userId, dto);
    }

    [HttpDelete("{matchId}/{userId}")]
    public async Task<Response<string>> DeletePlayerStats(long matchId, long userId)
    {
        return await _playerStatsService.Delete(matchId, userId);
    }
}
