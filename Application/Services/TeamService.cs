using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
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
                return new Response<string>(HttpStatusCode.NotFound, "Komanda ne naydenа!");

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
                return new Response<string>(HttpStatusCode.NotFound, "Komanda ne naydenа!");

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
            var teams = await _dbContext.Teams.ToListAsync();
            var response = new Response<List<Team>>(HttpStatusCode.OK, "Komandy polucheny");
            response.Data = teams;
            return response;
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
            var team = await _dbContext.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null)
                return new Response<Team>(HttpStatusCode.NotFound, "Komanda ne naydenа!");

            return new Response<Team>(HttpStatusCode.OK, "Komanda poluchena", team);
        }
        catch (Exception ex)
        {
            return new Response<Team>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
