using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        void Add(Movie movie);
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);
    }
}
