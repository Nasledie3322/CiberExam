using Domain.Models;
using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface ITeamMemberService
{
    Task<Response<string>> Add(AddTeamMemberDto teamMemberDto);
    Task<Response<string>> Delete(long teamId, long userId);
    Task<Response<List<TeamMember>>> GetTeamMembers();
    Task<Response<TeamMember>> GetTeamMemberById(long teamId, long userId);
}
