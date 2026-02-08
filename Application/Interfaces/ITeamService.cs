using Domain.Models;
using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface ITeamService
{
    Task<Response<string>> Add(AddTeamDto teamDto);
    Task<Response<string>> Update(long teamId, UpdateTeamDto teamDto);
    Task<Response<string>> Delete(long teamId);
    Task<Response<List<Team>>> GetTeams();
    Task<Response<Team>> GetTeamById(long teamId);
}
