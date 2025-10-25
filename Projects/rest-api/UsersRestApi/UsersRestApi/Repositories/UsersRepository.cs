using UsersRestApi.Entities;

namespace UsersRestApi.Repositories
{
    public class UsersRepository
    {
        public static List<User> Items { get; set; }

        // a static constructor completes when
        // someone calls a static resource for the first time in the app
        // It completes on its own - without the need of using the new operator, and completes only one time
        static UsersRepository()
        {
            Items = new List<User>();
        }
    }
}
