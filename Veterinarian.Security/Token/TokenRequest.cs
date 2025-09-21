namespace Veterinarian.Security.Token
{
    public sealed record TokenRequest(string UserId,string Email, IEnumerable<string> Roles);
}
