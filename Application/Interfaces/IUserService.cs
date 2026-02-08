using Domain.Models;
using Application.Responses;
using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<Response<string>> Add(AddUserDto userDto);
    Task<Response<string>> Update(int userId, UpdateUserDto userDto);
    Task<Response<string>> Delete(int userId);
    Task<Response<List<User>>> GetUsers();
    Task<Response<User>> GetUserById(int userId);
}
