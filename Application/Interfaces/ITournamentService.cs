using Domain.Models;
using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface ITournamentService
{
    Task<Response<string>> Add(AddTournamentDto tournamentDto);
    Task<Response<string>> Update(long tournamentId, UpdateTournamentDto tournamentDto);
    Task<Response<string>> Delete(long tournamentId);
    Task<Response<List<Tournament>>> GetTournaments();
    Task<Response<Tournament>> GetTournamentById(long tournamentId);
}
