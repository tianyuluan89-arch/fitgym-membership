using FitGym.Models;

namespace FitGym.Services
{
    public interface IMemberService
    {
        Member RegisterMember(string fullName, string email, string? phone, DateTime dateOfBirth);
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
    }
}
