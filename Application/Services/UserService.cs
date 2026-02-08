using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Infrastructure.Data;

namespace Application.Services;

public class UserService(IMapper mapper, ApplicationDbContext dbContext) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Response<string>> Add(AddUserDto userDto)
    {
        try
        {
            var user = _mapper.Map<User>(userDto);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Polzovatel dobavlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Update(int userId, UpdateUserDto userDto)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "Polzovatel ne nayden!");

            _mapper.Map(userDto, user);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Polzovatel obnovlen!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<string>> Delete(int userId)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return new Response<string>(HttpStatusCode.NotFound, "Polzovatel ne nayden!");

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return new Response<string>(HttpStatusCode.OK, "Polzovatel udaljon!");
        }
        catch (Exception ex)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<List<User>>> GetUsers()
    {
        try
        {
            var users = await _dbContext.Users.ToListAsync();
            var response = new Response<List<User>>(HttpStatusCode.OK, "Polzovateli polucheny");
            response.Data = users;
            return response;
        }
        catch (Exception ex)
        {
            return new Response<List<User>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<User>> GetUserById(int userId)
    {
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new Response<User>(HttpStatusCode.NotFound, "Polzovatel ne nayden!");

            return new Response<User>(HttpStatusCode.OK, "Polzovatel poluchen", user);
        }
        catch (Exception ex)
        {
            return new Response<User>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
