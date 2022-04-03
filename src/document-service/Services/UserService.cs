﻿using core.ServerResponse;
using document_service.Extensions;
using document_service.Helpers.JWT;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Responses;
using document_service.Repositories;

namespace document_service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IJwtHelper _jwtHelper;

    public UserService(IUserRepository repository, IJwtHelper jwtHelper)
    {
        _repository = repository;
        _jwtHelper = jwtHelper;
    }
    
    public async Task<Response<IEnumerable<UserResponse>>> GetAll()
    {
        var users = await _repository.GetAll();
        return new SuccessResponse<IEnumerable<UserResponse>>(users.ToUsersResponse());
    }

    public async Task<Response<UserResponse>> GetById(Guid id)
    {
        var user = await _repository.GetById(id);
        if (user is not null)
            return new SuccessResponse<UserResponse>(user.ToUserResponse());
        return new ErrorResponse<UserResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);

    }

    public async Task<Response<UserResponse>> GetByEmail(string request)
    {
        var user = await _repository.GetByEmail(request);
        if (user is not null)
            return new SuccessResponse<UserResponse>(user.ToUserResponse());
        return new ErrorResponse<UserResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);
    }

    public async Task<Response<string>> Create(CreateUserRequest request)
    {
        var result = GetByEmail(request.Email);
        if (result.Result.Success)
        {
            return new ErrorResponse<string>(ResponseStatus.BadRequest,default, ResultMessage.Error);
        }
        return new SuccessResponse<string>(await _repository.Create(request.ToUser())); 
    }

    public async Task<Response<bool>> Update(Guid id,UpdateUserRequest request)
    {
        var newUser = request.ToUser();
        var result = await _repository.Update(id, newUser);
        if (result)
        {
            return new SuccessResponse<bool>(result);
        }
            
        return new ErrorResponse<bool>(ResponseStatus.NotFound, result, ResultMessage.NotFoundUser);
    }

    public async Task<Response<bool>> Delete(Guid id)
    {
        
        var result = await _repository.Delete(id);
        if (result)
        {
            return new SuccessResponse<bool>(result);
        }

        return new ErrorResponse<bool>(ResponseStatus.NotFound, result, ResultMessage.NotFoundUser);
    }

    public async Task<Response<AccessTokenResponse>> Login(LoginRequest request)
    {
        var user = GetByEmail(request.Email);
        if (!user.Result.Success)
        {
            return new ErrorResponse<AccessTokenResponse>(ResponseStatus.NotFound,null,ResultMessage.Error);
        }

        if (user.Result.Data.Password != request.Password)
        {
            return new ErrorResponse<AccessTokenResponse>(ResponseStatus.NotFound,null,ResultMessage.Error);
        }

        var accessToken = _jwtHelper.GenereteJwtToken(user.Result.Data.Id, user.Result.Data.Role);
        
        return new SuccessResponse<AccessTokenResponse>(accessToken);
    }

    public Response<TokenHandlerResponse> ValidateToken(string request)
    {
        var result = _jwtHelper.ValidateJwtToken(request);
        if (result.Status)
        {
            return new SuccessResponse<TokenHandlerResponse>(result);
        }
        return new ErrorResponse<TokenHandlerResponse>(ResponseStatus.UnAuthorized,result,ResultMessage.UnAuthorized);
    }
}