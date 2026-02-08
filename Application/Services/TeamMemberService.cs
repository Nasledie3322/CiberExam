using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Infrastructure.Data;

namespace Application.Services;

public class TeamMemberService(IMapper mapper, ApplicationDbContext dbContext) : ITeamMemberService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Response<string>> Add(AddTeamMemberDto teamMemberDto)
    {
        try
        {
            var teamMember = _mapper.Map<TeamMember>(teamMemberDto);
            await _dbContext.TeamMembers.AddAsync(teamMember);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Chlen komandy dobavlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Delete(long teamId, long userId)
    {
        try
        {
            var teamMember = await _dbContext.TeamMembers.FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
            if (teamMember == null)
                return new Response<string>(HttpStatusCode.NotFound, "Chlen komandy ne nayden!");

            _dbContext.TeamMembers.Remove(teamMember);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Chlen komandy udaljon!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<List<TeamMember>>> GetTeamMembers()
    {
        try
        {
            var teamMembers = await _dbContext.TeamMembers.ToListAsync();
            var response = new Response<List<TeamMember>>(HttpStatusCode.OK, "Chleny komand polucheny");
            response.Data = teamMembers;
            return response;
        }
        catch (Exception ex)
        {
            return new Response<List<TeamMember>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<TeamMember>> GetTeamMemberById(long teamId, long userId)
    {
        try
        {
            var teamMember = await _dbContext.TeamMembers.FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
            if (teamMember == null)
                return new Response<TeamMember>(HttpStatusCode.NotFound, "Chlen komandy ne nayden!");

            return new Response<TeamMember>(HttpStatusCode.OK, "Chlen komandy poluchen", teamMember);
        }
        catch (Exception ex)
        {
            return new Response<TeamMember>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
