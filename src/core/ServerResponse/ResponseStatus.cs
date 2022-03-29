﻿namespace core.ServerResponse;

public enum ResponseStatus : int
{
    Ok = 200,
    Created = 201,
    BadRequest = 400,
    UnAuthorized = 401,
    Forbid = 403,
    NotFound = 404,
    Internal = 500
}