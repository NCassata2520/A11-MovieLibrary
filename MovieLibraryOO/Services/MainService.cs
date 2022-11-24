using ConsoleTables;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dao;
using MovieLibraryOO.Dto;
using MovieLibraryOO.Mappers;
using System;
using System.Linq;

namespace MovieLibraryOO.Services
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IMovieMapper _movieMapper;
        private readonly IRepository _repository;
        private readonly IFileService _fileService;

        public MainService(ILogger<MainService> logger, IMovieMapper movieMapper, IRepository repository, IFileService fileService)
        {
            _logger = logger;
            _movieMapper = movieMapper;
            _repository = repository;
            _fileService = fileService;
        }

        public void Invoke()
        {
            var menu = new Menu();

            Menu.MenuOptions menuChoice;
            do
            {
                menuChoice = menu.ChooseAction();

                switch (menuChoice)
                {
                    case Menu.MenuOptions.ListFromDb:
                        _logger.LogInformation("Listing movies from database");
                        var allMovies = _repository.GetAll();
                        var movies = _movieMapper.Map(allMovies);
                        ConsoleTable.From<MovieDto>(movies).Write();
                        break;
                    case Menu.MenuOptions.ListFromFile:
                        _fileService.Read();
                        _fileService.Display();
                        break;
                    case Menu.MenuOptions.Add:
                        System.Console.WriteLine("Enter new movie Name: ");
                        var movie2 = Console.ReadLine();

                        using (var db = new MovieContext())
                        {
                            db.Movies.Add(new Movie()
                                {
                                    Title = movie2
                                });
                                 db.SaveChanges();

                                 var newMovie = db.Movies.FirstOrDefault(x => x.Title == movie2);
                                    System.Console.WriteLine($"({newMovie.Id}) {newMovie.Title}");
                                    }

                        /*_logger.LogInformation("Adding a new movie");
                        
                        Console.WriteLine("What is the movie title?");
                            var movieTitle = Console.ReadLine();
                        Console.WriteLine("What is the release date?");
                        var movieReleaseDate = Console.ReadLine();
                        var movieRelease = DateTime.Parse(movieReleaseDate);

                        var movie = new Movie()
                        {
                            Title = movieTitle,
                            ReleaseDate = movieReleaseDate
                        }


                        _repository.Add(movie);
                        */
                        break;

                    case Menu.MenuOptions.Update:
                        System.Console.WriteLine("Enter Movie Name to Update: ");
                        var movie3 = Console.ReadLine();

                        System.Console.WriteLine("Enter Updated Movie Name: ");
                        var movieUpdate = Console.ReadLine();

                        using (var db = new MovieContext())
                        {
                        var updateMovie = db.Movies.FirstOrDefault(x => x.Title == movie3);
                         System.Console.WriteLine($"({updateMovie.Id}) {updateMovie.Title}");

                         updateMovie.Title = movieUpdate;

                            db.Movies.Update(updateMovie);
                            db.SaveChanges();

                        }
                       // _logger.LogInformation("Updating an existing movie");
                        
                        break;
                    case Menu.MenuOptions.Delete:
                        System.Console.WriteLine("Enter Movie Name to Delete: ");
                        var movie4 = Console.ReadLine();

                        using (var db = new MovieContext())
                        {
                        var deleteMovie = db.Movies.FirstOrDefault(x => x.Title == movie4);
                        System.Console.WriteLine($"({deleteMovie.Id}) {deleteMovie.Title}");

                         // verify exists first
                         db.Movies.Remove(deleteMovie);
                         db.SaveChanges();
                        }


                        //_logger.LogInformation("Deleting a movie");
                        break;
                    case Menu.MenuOptions.Search:
                        _logger.LogInformation("Searching for a movie");
                        var userSearchTerm = menu.GetUserResponse("Enter your", "search string:", "green");
                        var searchedMovies = _repository.Search(userSearchTerm);
                        movies = _movieMapper.Map(searchedMovies);
                        ConsoleTable.From<MovieDto>(movies).Write();
                        break;
                }
            }
            while (menuChoice != Menu.MenuOptions.Exit);

            menu.Exit();


            Console.WriteLine("\nThanks for using the Movie Library!");

        }
    }
}