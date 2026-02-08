using AutoMapper;
using Domain.Models;
using Application.DTOs;

namespace Application.MapperProfile;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<AddUserDto, User>();
        CreateMap<UpdateUserDto, User>();

        CreateMap<AddTeamDto, Team>();
        CreateMap<UpdateTeamDto, Team>();

        CreateMap<AddTeamMemberDto, TeamMember>();

        CreateMap<AddTournamentDto, Tournament>();
        CreateMap<UpdateTournamentDto, Tournament>();

        CreateMap<AddMatchDto, Match>();
        CreateMap<UpdateMatchDto, Match>();

        CreateMap<AddMatchResultDto, MatchResult>();
        CreateMap<UpdateMatchResultDto, MatchResult>();

        CreateMap<AddPlayerStatsDto, PlayerStats>();
        CreateMap<UpdatePlayerStatsDto, PlayerStats>();

        CreateMap<User, UserDto>();
        CreateMap<Team, TeamDto>();
        CreateMap<Tournament, TournamentDto>();
        CreateMap<Match, MatchDto>();
        CreateMap<MatchResult, MatchResultDto>();
        CreateMap<PlayerStats, PlayerStatsDto>();
        CreateMap<TeamMember, TeamMemberDto>();
    }
}