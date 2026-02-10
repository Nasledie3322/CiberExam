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
            var teamMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(x => x.TeamId == teamId && x.UserId == userId);

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
            var members = await _dbContext.TeamMembers
                .AsNoTracking()
                .ToListAsync();

            return new Response<List<TeamMember>>(HttpStatusCode.OK, "Chleny komand polucheny", members);
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
            var teamMember = await _dbContext.TeamMembers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TeamId == teamId && x.UserId == userId);

            if (teamMember == null)
                return new Response<TeamMember>(HttpStatusCode.NotFound, "Chlen komandy ne nayden!");

            return new Response<TeamMember>(HttpStatusCode.OK, "Chlen komandy poluchen", teamMember);
        }
        catch (Exception ex)
        {
            return new Response<TeamMember>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<PagedResult<TeamMember>>> GetTeamMembersPaged(PagedQuery query)
    {
        try
        {
            query.Page = query.Page < 1 ? 1 : query.Page;
            query.PageSize = query.PageSize < 1 ? 20 : query.PageSize;
            query.PageSize = query.PageSize > 100 ? 100 : query.PageSize;

            IQueryable<TeamMember> members =
                _dbContext.TeamMembers.AsNoTracking();

            var totalCount = await members.CountAsync();

            var items = await members
                .OrderBy(x => x.TeamId)
                .ThenBy(x => x.UserId)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<TeamMember>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };

            return new Response<PagedResult<TeamMember>>(HttpStatusCode.OK, "Chleny komand s paginaciey polucheny", result);
        }
        catch (Exception ex)
        {
            return new Response<PagedResult<TeamMember>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
