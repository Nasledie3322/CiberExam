using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Application.Filtering;
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
            var playerStats = await _dbContext.PlayerStats
                .FirstOrDefaultAsync(x => x.MatchId == matchId && x.UserId == userId);

            if (playerStats == null)
                return new Response<string>(HttpStatusCode.NotFound, "Statistika igroka ne naydena!");

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
            var playerStats = await _dbContext.PlayerStats
                .FirstOrDefaultAsync(x => x.MatchId == matchId && x.UserId == userId);

            if (playerStats == null)
                return new Response<string>(HttpStatusCode.NotFound, "Statistika igroka ne naydena!");

            _dbContext.PlayerStats.Remove(playerStats);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Statistika igroka udalena!");
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
            var stats = await _dbContext.PlayerStats
                .AsNoTracking()
                .ToListAsync();

            return new Response<List<PlayerStats>>(HttpStatusCode.OK, "Statistika igrokov poluchena", stats);
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
            var playerStats = await _dbContext.PlayerStats
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MatchId == matchId && x.UserId == userId);

            if (playerStats == null)
                return new Response<PlayerStats>(HttpStatusCode.NotFound, "Statistika igroka ne naydena!");

            return new Response<PlayerStats>(HttpStatusCode.OK, "Statistika igroka poluchena", playerStats);
        }
        catch (Exception ex)
        {
            return new Response<PlayerStats>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<PagedResult<PlayerStats>>> GetPlayerStatsPaged(PagedQuery query)
    {
        try
        {
            query.Page = query.Page < 1 ? 1 : query.Page;
            query.PageSize = query.PageSize < 1 ? 20 : query.PageSize;
            query.PageSize = query.PageSize > 100 ? 100 : query.PageSize;

            IQueryable<PlayerStats> stats = _dbContext.PlayerStats.AsNoTracking();

            var totalCount = await stats.CountAsync();

            var items = await stats
                .OrderByDescending(x => x.MatchId)
                .ThenBy(x => x.UserId)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<PlayerStats>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };

            return new Response<PagedResult<PlayerStats>>(HttpStatusCode.OK, "Statistika s paginaciey poluchena", result);
        }
        catch (Exception ex)
        {
            return new Response<PagedResult<PlayerStats>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
