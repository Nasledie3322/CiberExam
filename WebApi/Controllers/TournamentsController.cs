using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Application.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public TournamentsController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    [HttpGet]
    public async Task<Response<List<Tournament>>> GetTournaments()
    {
        return await _tournamentService.GetTournaments();
    }

    [HttpGet("{id}")]
    public async Task<Response<Tournament>> GetTournamentById(long id)
    {
        return await _tournamentService.GetTournamentById(id);
    }

    [HttpPost]
    public async Task<Response<string>> AddTournament(AddTournamentDto dto)
    {
        return await _tournamentService.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateTournament(long id, UpdateTournamentDto dto)
    {
        return await _tournamentService.Update(id, dto);
    }

    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteTournament(long id)
    {
        return await _tournamentService.Delete(id);
    }
}
