using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Infrastructure.Data;

namespace Application.Services;

public class MatchService(IMapper mapper, ApplicationDbContext dbContext) : IMatchService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Response<string>> Add(AddMatchDto matchDto)
    {
        try
        {
            var match = _mapper.Map<Match>(matchDto);
            await _dbContext.Matches.AddAsync(match);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Match dobavlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Update(long matchId, UpdateMatchDto matchDto)
    {
        try
        {
            var match = await _dbContext.Matches.FindAsync(matchId);
            if (match == null)
                return new Response<string>(HttpStatusCode.NotFound, "Match ne nayden!");

            _mapper.Map(matchDto, match);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Match obnovlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Delete(long matchId)
    {
        try
        {
            var match = await _dbContext.Matches.FindAsync(matchId);
            if (match == null)
                return new Response<string>(HttpStatusCode.NotFound, "Match ne nayden!");

            _dbContext.Matches.Remove(match);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Match udaljon!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<List<Match>>> GetMatches()
    {
        try
        {
            var matches = await _dbContext.Matches.ToListAsync();
            var response = new Response<List<Match>>(HttpStatusCode.OK, "Matchy polucheny");
            response.Data = matches;
            return response;
        }
        catch (Exception ex)
        {
            return new Response<List<Match>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<Match>> GetMatchById(long matchId)
    {
        try
        {
            var match = await _dbContext.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
            if (match == null)
                return new Response<Match>(HttpStatusCode.NotFound, "Match ne nayden!");

            return new Response<Match>(HttpStatusCode.OK, "Match poluchen", match);
        }
        catch (Exception ex)
        {
            return new Response<Match>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
