using FitGym.Services;
using Xunit;

namespace FitGym.Tests
{
    public class MemberServiceTests
    {
        // Registering a member stores it and returns the created member
        [Fact]
        public void RegisterMember_ValidData_CreatesMember()
        {
            var service = new MemberService();
            var member = service.RegisterMember("John Doe", "john@example.com", null, DateTime.Today.AddYears(-25));
            Assert.NotNull(member);
            Assert.Equal("John Doe", member.FullName);
            Assert.Equal("Active", member.Status);
        }

        // Registering with an existing email throws DUPLICATE_EMAIL
        [Fact]
        public void RegisterMember_DuplicateEmail_ThrowsDuplicateEmail()
        {
            var service = new MemberService();
            service.RegisterMember("John Doe", "john@example.com", null, DateTime.Today.AddYears(-25));
            var ex = Assert.Throws<InvalidOperationException>(
                () => service.RegisterMember("Jane Doe", "john@example.com", null, DateTime.Today.AddYears(-22)));
            Assert.Equal("DUPLICATE_EMAIL", ex.Message);
        }
    }
}
