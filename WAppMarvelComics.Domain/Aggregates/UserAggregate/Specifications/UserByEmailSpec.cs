using Ardalis.Specification;

namespace WAppMarvelComics.Domain.Aggregates.UserAggregate.Specifications
{
    public class UserByEmailSpec : Specification<User>
    {
        public UserByEmailSpec(string email)
        {
            Query
                .Where(us => us.Email.ToUpper() == email.ToUpper()).Include(cd => cd.ComicFavorites);
        }
    }
}