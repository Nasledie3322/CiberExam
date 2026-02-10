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

public class TournamentService(IMapper mapper, ApplicationDbContext dbContext) : ITournamentService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Response<string>> Add(AddTournamentDto tournamentDto)
    {
        try
        {
            var tournament = _mapper.Map<Tournament>(tournamentDto);
            await _dbContext.Tournaments.AddAsync(tournament);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Turnir dobavlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Update(long tournamentId, UpdateTournamentDto tournamentDto)
    {
        try
        {
            var tournament = await _dbContext.Tournaments.FindAsync(tournamentId);
            if (tournament == null)
                return new Response<string>(HttpStatusCode.NotFound, "Turnir ne nayden!");

            _mapper.Map(tournamentDto, tournament);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Turnir obnovlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Delete(long tournamentId)
    {
        try
        {
            var tournament = await _dbContext.Tournaments.FindAsync(tournamentId);
            if (tournament == null)
                return new Response<string>(HttpStatusCode.NotFound, "Turnir ne nayden!");

            _dbContext.Tournaments.Remove(tournament);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Turnir udaljon!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<List<Tournament>>> GetTournaments()
    {
        try
        {
            var tournaments = await _dbContext.Tournaments
                .AsNoTracking()
                .ToListAsync();

            return new Response<List<Tournament>>(HttpStatusCode.OK, "Turniry polucheny", tournaments);
        }
        catch (Exception ex)
        {
            return new Response<List<Tournament>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<Tournament>> GetTournamentById(long tournamentId)
    {
        try
        {
            var tournament = await _dbContext.Tournaments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == tournamentId);

            if (tournament == null)
                return new Response<Tournament>(HttpStatusCode.NotFound, "Turnir ne nayden!");

            return new Response<Tournament>(HttpStatusCode.OK, "Turnir poluchen", tournament);
        }
        catch (Exception ex)
        {
            return new Response<Tournament>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<PagedResult<Tournament>>> GetTournamentsPaged(PagedQuery query)
    {
        try
        {
            query.Page = query.Page < 1 ? 1 : query.Page;
            query.PageSize = query.PageSize < 1 ? 20 : query.PageSize;
            query.PageSize = query.PageSize > 100 ? 100 : query.PageSize;

            IQueryable<Tournament> tournaments =
                _dbContext.Tournaments.AsNoTracking();

            var totalCount = await tournaments.CountAsync();

            var items = await tournaments
                .OrderByDescending(x => x.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<Tournament>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };

            return new Response<PagedResult<Tournament>>(HttpStatusCode.OK, "Turniry s paginaciey polucheny", result);
        }
        catch (Exception ex)
        {
            return new Response<PagedResult<Tournament>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
