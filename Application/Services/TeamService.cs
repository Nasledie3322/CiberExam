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

public class TeamService(IMapper mapper, ApplicationDbContext dbContext) : ITeamService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Response<string>> Add(AddTeamDto teamDto)
    {
        try
        {
            var team = _mapper.Map<Team>(teamDto);
            await _dbContext.Teams.AddAsync(team);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Komanda dobavlena!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Update(long teamId, UpdateTeamDto teamDto)
    {
        try
        {
            var team = await _dbContext.Teams.FindAsync(teamId);
            if (team == null)
                return new Response<string>(HttpStatusCode.NotFound, "Komanda ne naydena!");

            _mapper.Map(teamDto, team);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Komanda obnovlena!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Delete(long teamId)
    {
        try
        {
            var team = await _dbContext.Teams.FindAsync(teamId);
            if (team == null)
                return new Response<string>(HttpStatusCode.NotFound, "Komanda ne naydena!");

            _dbContext.Teams.Remove(team);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Komanda udaljena!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<List<Team>>> GetTeams()
    {
        try
        {
            var teams = await _dbContext.Teams
                .AsNoTracking()
                .ToListAsync();

            return new Response<List<Team>>(HttpStatusCode.OK, "Komandy polucheny", teams);
        }
        catch (Exception ex)
        {
            return new Response<List<Team>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<Team>> GetTeamById(long teamId)
    {
        try
        {
            var team = await _dbContext.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == teamId);

            if (team == null)
                return new Response<Team>(HttpStatusCode.NotFound, "Komanda ne naydena!");

            return new Response<Team>(HttpStatusCode.OK, "Komanda poluchena", team);
        }
        catch (Exception ex)
        {
            return new Response<Team>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<PagedResult<Team>>> GetTeamsPaged(PagedQuery query)
    {
        try
        {
            query.Page = query.Page < 1 ? 1 : query.Page;
            query.PageSize = query.PageSize < 1 ? 20 : query.PageSize;
            query.PageSize = query.PageSize > 100 ? 100 : query.PageSize;

            IQueryable<Team> teams =
                _dbContext.Teams.AsNoTracking();

            var totalCount = await teams.CountAsync();

            var items = await teams
                .OrderByDescending(x => x.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<Team>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };

            return new Response<PagedResult<Team>>(HttpStatusCode.OK, "Komandy s paginaciey polucheny", result);
        }
        catch (Exception ex)
        {
            return new Response<PagedResult<Team>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
