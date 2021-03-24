using Assignment9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment9.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private FilmContext _context { get; set; }

        public HomeController(ILogger<HomeController> logger, FilmContext context)
        {
            _logger = logger;
            _context = context;
        }

        //home view
        public IActionResult Index()
        {
            return View();
        }

        //podcast link view
        public IActionResult MyPodcasts()
        {
            return View();
        }

        //add film view
        [HttpGet]
        public IActionResult AddFilm()
        {
            return View();
        }

        //upon form post submission, AddFilm action
        [HttpPost]
        public IActionResult AddFilm(FilmModel film)
        {
            //if input is valid
            if (ModelState.IsValid)
            {
                //Add and Save data
                _context.Films.Add(film);
                _context.SaveChanges();

                //return view for confirmation of the added film
                return View("ConfirmSubmission", film);
            }
            //if input is invalid
            else
            {
                //return current view
                return View();
            }
        }

        //List films
        [HttpGet]
        public IActionResult ListFilms()
        {
            return View(_context.Films);
        }
        
        //return user to view for editing a film's information
        [HttpPost]
        public IActionResult EditFilm(int filmId)
        {
            FilmModel film = _context.Films.Single(x => x.FilmId == filmId);

            return View("EditFilm", film);
        }

        //save changes user made in EditFilm view
        [HttpPost]
        public IActionResult SaveEdits(FilmModel film, int filmId)
        {
            
            _context.Films.Single(x => x.FilmId == filmId).Category = film.Category;
            _context.Films.Single(x => x.FilmId == filmId).Title = film.Title;
            _context.Films.Single(x => x.FilmId == filmId).Year = film.Year;
            _context.Films.Single(x => x.FilmId == filmId).Rating = film.Rating;
            _context.Films.Single(x => x.FilmId == filmId).Edited = film.Edited;
            _context.Films.Single(x => x.FilmId == filmId).LentTo = film.LentTo;
            _context.Films.Single(x => x.FilmId == filmId).Notes = film.Notes;
            
            //_context.Films.Update(film);
            _context.SaveChanges();
            return View("ListFilms", _context.Films);
        }
        
        //Ask user to confirm they would like to delete a specified film
        [HttpPost]
        public IActionResult RatifyDeletion(int filmId)
        {
            FilmModel film = _context.Films.Single(x => x.FilmId == filmId);
            return View(film);
        }
        
        //delete film after user has ratified the decision
        [HttpPost]
        public IActionResult DeleteFilm(int filmId)
        {
            FilmModel film = _context.Films.Single(x => x.FilmId == filmId);
            _context.Films.Remove(film);
            _context.SaveChanges();
            return View("ListFilms", _context.Films);
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
