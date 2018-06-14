using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GASPG.Data;
using GASPG.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using GASPG.Models.ViewModel.GameViewModels;
using GASPG.Helpers;

namespace GASPG.Controllers
{
    public class GamesController : Controller
    {
        private readonly GASPGDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public GamesController(GASPGDbContext context, IMapper mapper,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Games
        [AllowAnonymous]
        public IActionResult Error(int? statusCode)
        {
            var vm = new ErrorViewModel
            {
                Response = statusCode?.ToString() ?? "-",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(vm);
        }

        // GET: Game
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var games = await GetGamesGreedy()
                .Select(m => _mapper.Map<GameViewModel>(m))
                .ToListAsync();

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await GetCurrentUser();

                // make each offer edit-able when user is its author OR admin/moderator
                games.ForEach(m => m.CanEdit = (m.Author.Id == user.Id) || UserIsAdministrator() || UserIsModerator());
            }

            ViewData["GameCount"] = games.Count;
            return View(games);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                return RedirectToAction(nameof(Index));
            }

            phrase = phrase.ToLower();

            // select each offer that contains search phrase in given fields, then map it to view models list
            var games = await GetGamesGreedy()
                .Where(c => c.Title.Contains(phrase)
                || c.Description.ToLower().Contains(phrase)
                || c.Developer.Name.ToLower().Contains(phrase)
                || c.Genre.Name.ToLower().Contains(phrase)
                || c.Author.Email.ToLower().Contains(phrase))
                .Select(m => _mapper.Map<GameViewModel>(m))
                .ToListAsync();

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await GetCurrentUser();

                // make each offer edit-able when user is its author OR admin/moderator
                games.ForEach(m => m.CanEdit = (m.Author.Id == user.Id) || UserIsAdministrator() || UserIsModerator());
            }

            // pass job offer count and search phrase to the view
            ViewData["GameCount"] = games.Count;
            ViewData["phrase"] = phrase;

            return View("Index", games);
        }

        // GET: Game/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var game = await GetGamesGreedy()
                .SingleOrDefaultAsync(m => m.GameId == id);

            if (game == null)
            {
                return View("NotFound");
            }

            _context.Update(game);
            await _context.SaveChangesAsync();

            var viewModel = _mapper.Map<DetailsGameViewModel>(game);

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var user = await GetCurrentUser();

                // make each offer edit-able when user is its author OR admin/moderator
                viewModel.CanEdit = (viewModel.Author.Id == user.Id) || UserIsAdministrator() || UserIsModerator();
            }

            return View(viewModel);
        }

        // GET: Game/Create
        public IActionResult Create()
        {
            var viewModel = new CreateGameViewModel
            {
                Genres = _context.Genres,
                Developers = _context.Developers
            };

            return View(viewModel);
        }

        // POST: Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _context.Genres;
                model.Developers = _context.Developers;
                return View(model);
            }

            var game = new Game
            {
                Author = await GetCurrentUser(),
                Genre = await _context.Genres.SingleOrDefaultAsync(c => c.GenreId == model.GenreId),
                Developer = await _context.Developers.SingleOrDefaultAsync(c => c.DeveloperId == model.DeveloperId),
                PEGI = model.PEGI,
                Title = model.Title,
                Description = model.Description,
                Realeased = DateTime.Now,
                Price = model.Price
            };

            _context.Add(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var game = await GetGamesGreedy()
                .SingleOrDefaultAsync(m => m.GameId == id);

            if (game == null)
            {
                return View("NotFound");
            }

            if (game.Author.Id != (await GetCurrentUser()).Id && !UserIsModerator() && !UserIsAdministrator())
            {
                return View("AccessDenied");
            }

            var viewModel = _mapper.Map<EditGameViewModel>(game);
            viewModel.Genres = _context.Genres;
            viewModel.Developers = _context.Developers;
            return View(viewModel);
        }

        // POST: Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _context.Genres;
                model.Developers = _context.Developers;
                return View(model);
            }

            try
            {
                var game = await _context.Games
                    .SingleOrDefaultAsync(m => m.GameId == model.GameId);

                game.Genre = await _context.Genres
                    .SingleOrDefaultAsync(c => c.GenreId == model.GenreId);
                game.Developer = await _context.Developers
                    .SingleOrDefaultAsync(c => c.DeveloperId == model.DeveloperId);
                game.PEGI = model.PEGI;
                game.Title = model.Title;
                game.Description = model.Description;
                game.Price = model.Price;

                _context.Update(game);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(model.GameId))
                {
                    return View("NotFound");
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var game = await _context.Games
                .SingleOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<DeleteGameViewModel>(game);
            return View(viewModel);
        }

        // POST: Game/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var game = await _context.Games
                .SingleOrDefaultAsync(m => m.GameId == model.GameId);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(string id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }

        private IQueryable<Game> GetGamesGreedy()
        {
            // greedy load job offers (with all connected data)
            // as EntityFramework Core does not support lazy-loading yet
            return _context.Games
                .Include(u => u.Genre)
                .Include(u => u.Developer)
                .Include(u => u.Author);
        }

        private async Task<AppUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        private bool UserIsModerator()
        {
            var user = GetCurrentUser().Result;
            return _userManager.IsInRoleAsync(user, RoleHelper.Moderator).Result;
        }

        private bool UserIsAdministrator()
        {
            var user = GetCurrentUser().Result;
            return _userManager.IsInRoleAsync(user, RoleHelper.Administrator).Result;
        }
    }
}
