using FitGym.Models;

namespace FitGym.Services
{
    public class MemberService : IMemberService
    {
        private readonly List<Member> _members = new();

        public Member RegisterMember(string fullName, string email, string? phone, DateTime dateOfBirth)
        {
            // Reject duplicate email (HTTP 409 mapped by the controller)
            if (_members.Any(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("DUPLICATE_EMAIL");
            }

            var member = new Member
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                DateOfBirth = dateOfBirth,
                RegisteredAt = DateTime.UtcNow,
                Status = "Active"
            };
            _members.Add(member);
            return member;
        }

        public IEnumerable<Member> GetAll()
        {
            return _members;
        }

        public Member? GetById(int id)
        {
            return _members.FirstOrDefault(m => m.Id == id);
        }
    }
}
