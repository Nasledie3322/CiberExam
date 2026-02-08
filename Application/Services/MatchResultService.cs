using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
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
            var matchResult = await _dbContext.MatchResults.FirstOrDefaultAsync(mr => mr.MatchId == matchId && mr.TeamId == teamId);
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
            var matchResult = await _dbContext.MatchResults.FirstOrDefaultAsync(mr => mr.MatchId == matchId && mr.TeamId == teamId);
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

    public async Task<Response<List<MatchResult>>> GetMatchResults()
    {
        try
        {
            var matchResults = await _dbContext.MatchResults.ToListAsync();
            var response = new Response<List<MatchResult>>(HttpStatusCode.OK, "Rezultaty matchey polucheny");
            response.Data = matchResults;
            return response;
        }
        catch (Exception ex)
        {
            return new Response<List<MatchResult>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<MatchResult>> GetMatchResultById(long matchId, long teamId)
    {
        try
        {
            var matchResult = await _dbContext.MatchResults.FirstOrDefaultAsync(mr => mr.MatchId == matchId && mr.TeamId == teamId);
            if (matchResult == null)
                return new Response<MatchResult>(HttpStatusCode.NotFound, "Rezultat matcha ne nayden!");

            return new Response<MatchResult>(HttpStatusCode.OK, "Rezultat matcha poluchen", matchResult);
        }
        catch (Exception ex)
        {
            return new Response<MatchResult>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
