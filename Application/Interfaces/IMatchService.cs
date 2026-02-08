using Domain.Models;
using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IMatchService
{
    Task<Response<string>> Add(AddMatchDto matchDto);
    Task<Response<string>> Update(long matchId, UpdateMatchDto matchDto);
    Task<Response<string>> Delete(long matchId);
    Task<Response<List<Match>>> GetMatches();
    Task<Response<Match>> GetMatchById(long matchId);
}
