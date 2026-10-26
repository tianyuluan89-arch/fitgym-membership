namespace FitGym.Services
{
    public interface ITokenService
    {
        string CreateToken(int userId, string role);
    }
}
