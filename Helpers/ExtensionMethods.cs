using System.Collections.Generic;
using System.Linq;
using WEB_API.models;

namespace WEB_API.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Users> WithoutPasswords(this IEnumerable<Users> users) {
            return users.Select(x => x.WithoutPassword());
        }

        public static Users WithoutPassword(this Users user) {
            user.Password = null;
            return user;
        }
    }
}