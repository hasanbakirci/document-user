using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace user_service.Helper;

public class JwtHelper : IJwtHelper
{
    private readonly IConfiguration _configuration;
    private TokenOptions _tokenOptions;
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
    }
    public AccessTokenResponse GenereteJwtToken(Guid id, string role)
    {
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenOptions.SecurityKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", id.ToString()),
                new Claim("role", role)
            }),
            Issuer = _tokenOptions.Issuer,
            Audience = _tokenOptions.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var jwt = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(jwt);
        return new AccessTokenResponse { Token = token,Expiration = _accessTokenExpiration};

    }

    public TokenHandlerResponse ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenOptions.SecurityKey);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = _tokenOptions.Issuer,
                ValidAudience = _tokenOptions.Audience,
                ClockSkew = TimeSpan.Zero
            }, out  SecurityToken validatedToken );
        }
        catch
        {
            return GetClaims(token, false);
        }
        return GetClaims(token,true);

    }
    
    
    private static TokenHandlerResponse GetClaims(string token, bool status)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        return new TokenHandlerResponse
        {
            Status = status,
            Id = securityToken?.Claims.First(c => c.Type == "id").ToString().Split("id: ")[1],
            Role = securityToken?.Claims.First(c => c.Type == "role").ToString().Split("role: ")[1],
            ValidTo = securityToken?.ValidTo.Subtract(DateTime.UtcNow).ToString()
        };
    }

}