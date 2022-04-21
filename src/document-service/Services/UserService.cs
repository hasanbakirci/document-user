using core.Exceptions.CommonExceptions;
using core.ServerResponse;
using core.Validation;
using document_service.Extensions;
using document_service.Helpers.JWT;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Requests.RequestValidations;
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
        //var user = await _repository.GetBy(u => u.Id == id);
        var user = await _repository.GetById(id);
        if (user is not null)
        {
            return new SuccessResponse<UserResponse>(user.ToUserResponse());
        }
        //return new ErrorResponse<UserResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);
        throw new DocumentNotFound(id);

    }

    public async Task<Response<UserResponse>> GetByEmail(string request)
    {
        //var user = await _repository.GetBy(u => u.Email.ToLower() == request.ToLower());
        var user = await _repository.GetByEmail(request);
        if (user is not null)
        {
            return new SuccessResponse<UserResponse>(user.ToUserResponse());
        }
        return new ErrorResponse<UserResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);
    }

    public async Task<Response<string>> Create(CreateUserRequest request)
    {
        ValidationTool.Validate(new CreateUserRequestValidator(),request);
        
        var user = await GetByEmail(request.Email);
        if (user.Success)
        {
            //return new ErrorResponse<string>(ResponseStatus.BadRequest,default, ResultMessage.Error);
            throw new DuplicateKeyException(request.Email);
        }
        var result = await _repository.InsertOne(request.ToUser());
        return new SuccessResponse<string>(result.Id.ToString()); 
    }

    public async Task<Response<bool>> Update(Guid id,UpdateUserRequest request)
    {
        ValidationTool.Validate(new UpdateUserRequestValidator(),request);
        
        var newUser = request.ToUser();
        // var result = await _repository.UpdateOne(
        //     u => u.Id == id,
        //     (u => u.Username, request.Username),
        //     (u => u.Password, request.Password),
        //     (u => u.Role, request.Role)
        //     );
        var result = await _repository.UpdateUser(id, newUser);
        
        if (result)
        {
            return new SuccessResponse<bool>(true);
        }
        
        //return new ErrorResponse<bool>(ResponseStatus.NotFound, result, ResultMessage.NotFoundUser);
        throw new DocumentNotFound(id);
    }

    public async Task<Response<bool>> Delete(Guid id)
    {
        
        //var result = await _repository.DeleteOne(u => u.Id == id);
        var result = await _repository.DeleteById(id);
        if (result)
        {
            return new SuccessResponse<bool>(true);
        }
        
        //return new ErrorResponse<bool>(ResponseStatus.NotFound, result, ResultMessage.NotFoundUser);
        throw new DocumentNotFound(id);
    }

    public async Task<Response<AccessTokenResponse>> Login(LoginRequest request)
    {
        ValidationTool.Validate(new LoginRequestValidator(),request);
        
        var user = GetByEmail(request.Email);
        if (!user.Result.Success)
        {
            //return new ErrorResponse<AccessTokenResponse>(ResponseStatus.NotFound,null,ResultMessage.Error);
            throw new LoginException();
        }

        if (user.Result.Data.Password != request.Password)
        {
            //return new ErrorResponse<AccessTokenResponse>(ResponseStatus.NotFound,null,ResultMessage.Error);
            throw new LoginException();
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
        //return new ErrorResponse<TokenHandlerResponse>(ResponseStatus.UnAuthorized,result,ResultMessage.UnAuthorized);
        throw new InvalidTokenException();
    }
}