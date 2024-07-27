using Ardalis.Specification;

namespace WAppMarvelComics.Domain.Aggregates.UserAggregate.Specifications
{
    public class UserByEmailSpec : Specification<User>
    {
        public UserByEmailSpec(string email, string password)
        {
            Query
                .Where(us => us.Email.ToUpper() == email.ToUpper() 
                    && us.Password.ToUpper() == password.ToUpper());
        }
    }
}