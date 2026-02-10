using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Models;
using Application.Interfaces;
using Application.Responses;
using Application.DTOs;
using Application.Filtering;
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
            var users = await _dbContext.Users
                .AsNoTracking()
                .ToListAsync();
            return new Response<List<User>>(HttpStatusCode.OK, "Polzovateli polucheny", users);
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
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new Response<User>(HttpStatusCode.NotFound, "Polzovatel ne nayden!");
            return new Response<User>(HttpStatusCode.OK, "Polzovatel poluchen", user);
        }
        catch (Exception ex)
        {
            return new Response<User>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<PagedResult<User>>> GetUsersPaged(PagedQuery query)
    {
        try
        {
            query.Page = query.Page < 1 ? 1 : query.Page;
            query.PageSize = query.PageSize < 1 ? 20 : query.PageSize;
            query.PageSize = query.PageSize > 100 ? 100 : query.PageSize;

            IQueryable<User> users = _dbContext.Users.AsNoTracking();
            var totalCount = await users.CountAsync();

            var items = await users
                .OrderBy(u => u.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new PagedResult<User>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };

            return new Response<PagedResult<User>>(HttpStatusCode.OK, "Polzovateli s paginaciey polucheny", result);
        }
        catch (Exception ex)
        {
            return new Response<PagedResult<User>>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
