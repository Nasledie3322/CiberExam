using Domain.Models;
using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IPlayerStatsService
{
    Task<Response<string>> Add(AddPlayerStatsDto playerStatsDto);
    Task<Response<string>> Update(long matchId, long userId, UpdatePlayerStatsDto playerStatsDto);
    Task<Response<string>> Delete(long matchId, long userId);
    Task<Response<List<PlayerStats>>> GetPlayerStats();
    Task<Response<PlayerStats>> GetPlayerStatsById(long matchId, long userId);
}
