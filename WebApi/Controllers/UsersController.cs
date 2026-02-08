using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Application.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<Response<List<User>>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    [HttpGet("{id}")]
    public async Task<Response<User>> GetUserById(int id)
    {
        return await _userService.GetUserById(id);
    }

    [HttpPost]
    public async Task<Response<string>> AddUser(AddUserDto dto)
    {
        return await _userService.Add(dto);
    }

    [HttpPut("{id}")]
    public async Task<Response<string>> UpdateUser(int id, UpdateUserDto dto)
    {
        return await _userService.Update(id, dto);
    }

    [HttpDelete("{id}")]
    public async Task<Response<string>> DeleteUser(int id)
    {
        return await _userService.Delete(id);
    }
}
