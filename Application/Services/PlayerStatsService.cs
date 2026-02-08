using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Infrastructure.Data;

namespace Application.Services;

public class PlayerStatsService(IMapper mapper, ApplicationDbContext dbContext) : IPlayerStatsService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Response<string>> Add(AddPlayerStatsDto playerStatsDto)
    {
        try
        {
            var playerStats = _mapper.Map<PlayerStats>(playerStatsDto);
            await _dbContext.PlayerStats.AddAsync(playerStats);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Statistika igroka dobavlena!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Update(long matchId, long userId, UpdatePlayerStatsDto playerStatsDto)
    {
        try
        {
            var playerStats = await _dbContext.PlayerStats.FirstOrDefaultAsync(ps => ps.MatchId == matchId && ps.UserId == userId);
            if (playerStats == null)
                return new Response<string>(HttpStatusCode.NotFound, "Statistika igroka ne naydenа!");

            _mapper.Map(playerStatsDto, playerStats);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Statistika igroka obnovlena!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Delete(long matchId, long userId)
    {
        try
        {
            var playerStats = await _dbContext.PlayerStats.FirstOrDefaultAsync(ps => ps.MatchId == matchId && ps.UserId == userId);
            if (playerStats == null)
                return new Response<string>(HttpStatusCode.NotFound, "Statistika igroka ne naydenа!");

            _dbContext.PlayerStats.Remove(playerStats);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Statistika igroka udaljena!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<List<PlayerStats>>> GetPlayerStats()
    {
        try
        {
            var playerStats = await _dbContext.PlayerStats.ToListAsync();
            var response = new Response<List<PlayerStats>>(HttpStatusCode.OK, "Statistika igrokov poluchena");
            response.Data = playerStats;
            return response;
        }
        catch (Exception ex)
        {
            return new Response<List<PlayerStats>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<PlayerStats>> GetPlayerStatsById(long matchId, long userId)
    {
        try
        {
            var playerStats = await _dbContext.PlayerStats.FirstOrDefaultAsync(ps => ps.MatchId == matchId && ps.UserId == userId);
            if (playerStats == null)
                return new Response<PlayerStats>(HttpStatusCode.NotFound, "Statistika igroka ne naydenа!");

            return new Response<PlayerStats>(HttpStatusCode.OK, "Statistika igroka poluchena", playerStats);
        }
        catch (Exception ex)
        {
            return new Response<PlayerStats>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
