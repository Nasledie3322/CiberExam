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

public class MatchResultService(IMapper mapper, ApplicationDbContext dbContext) : IMatchResultService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Response<string>> Add(AddMatchResultDto matchResultDto)
    {
        try
        {
            var matchResult = _mapper.Map<MatchResult>(matchResultDto);
            await _dbContext.MatchResults.AddAsync(matchResult);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Rezultat matcha dobavlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Update(long matchId, long teamId, UpdateMatchResultDto matchResultDto)
    {
        try
        {
            var matchResult = await _dbContext.MatchResults
                .FirstOrDefaultAsync(x => x.MatchId == matchId && x.TeamId == teamId);

            if (matchResult == null)
                return new Response<string>(HttpStatusCode.NotFound, "Rezultat matcha ne nayden!");

            _mapper.Map(matchResultDto, matchResult);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Rezultat matcha obnovlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Delete(long matchId, long teamId)
    {
        try
        {
            var matchResult = await _dbContext.MatchResults
                .FirstOrDefaultAsync(x => x.MatchId == matchId && x.TeamId == teamId);

            if (matchResult == null)
                return new Response<string>(HttpStatusCode.NotFound, "Rezultat matcha ne nayden!");

            _dbContext.MatchResults.Remove(matchResult);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Rezultat matcha udaljon!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<PagedResult<MatchResult>>> GetMatchResultsPaged(PagedQuery query)
    {
        try
        {
            query.Page = query.Page < 1 ? 1 : query.Page;
            query.PageSize = query.PageSize < 1 ? 20 : query.PageSize;
            query.PageSize = query.PageSize > 100 ? 100 : query.PageSize;

            IQueryable<MatchResult> matchResults =
                _dbContext.MatchResults.AsNoTracking();

            var totalCount = await matchResults.CountAsync();

            var items = await matchResults
                .OrderByDescending(x => x.MatchId)
                .ThenBy(x => x.TeamId)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<MatchResult>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };

            return new Response<PagedResult<MatchResult>>(HttpStatusCode.OK, "Rezultaty polucheny", result);
        }
        catch (Exception ex)
        {
            return new Response<PagedResult<MatchResult>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<MatchResult>> GetMatchResultById(long matchId, long teamId)
    {
        try
        {
            var matchResult = await _dbContext.MatchResults
                .FirstOrDefaultAsync(x => x.MatchId == matchId && x.TeamId == teamId);

            if (matchResult == null)
                return new Response<MatchResult>(HttpStatusCode.NotFound, "Rezultat matcha ne nayden!");

            return new Response<MatchResult>(HttpStatusCode.OK, "Rezultat matcha poluchen", matchResult);
        }
        catch (Exception ex)
        {
            return new Response<MatchResult>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public Task<Response<List<MatchResult>>> GetMatchResults()
    {
        throw new NotImplementedException();
    }
}
