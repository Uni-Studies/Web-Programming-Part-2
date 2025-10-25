using MvcApiSample.Entities;

namespace MvcApiSample.Repositories
{
    public class UserRepository
    {
        public static List<User> Items {get; set;}

        // a static constructor completes when
        // someone calls a static resource for the first time in the app
        // It completes on its own - without the need of using the new operator, and completes only one time
        static UserRepository()
        {
            Items = new List<User>();
        }
    }
}
