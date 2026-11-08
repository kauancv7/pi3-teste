namespace WebApplicationsgls.Application.Services;

/// <summary>JWT claim names and the allowed subject types for the API.</summary>
public static class TokenClaims
{
    public const string Roles = "roles";
    public const string AccessType = "access_type";
    public const string InternalUser = "usuario";
    public const string Client = "cliente";
}
