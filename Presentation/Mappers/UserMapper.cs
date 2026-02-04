using Domain.Entities;
using Presentation.DTO;

namespace Presentation.Mappers
{
    public static class UserMapper
    {
        public static UserResponse ToResponse(User user) => new UserResponse(user.Login);
        public static List<UserResponse> ToResponseList(IEnumerable<User> users) => users.Select(ToResponse).ToList();
    }
}
