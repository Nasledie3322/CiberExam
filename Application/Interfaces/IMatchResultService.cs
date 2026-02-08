using Domain.Models;
using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IMatchResultService
{
    Task<Response<string>> Add(AddMatchResultDto matchResultDto);
    Task<Response<string>> Update(long matchId, long teamId, UpdateMatchResultDto matchResultDto);
    Task<Response<string>> Delete(long matchId, long teamId);
    Task<Response<List<MatchResult>>> GetMatchResults();
    Task<Response<MatchResult>> GetMatchResultById(long matchId, long teamId);
}
