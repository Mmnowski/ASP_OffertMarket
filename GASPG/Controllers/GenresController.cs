using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GASPG.Data;
using GASPG.Models;
using Microsoft.AspNetCore.Authorization;
using GASPG.Helpers;
using GASPG.Models.ViewModel.GenreViewModels;
using AutoMapper;

namespace GASPG.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class GenresController : Controller
    {
        private readonly GASPGDbContext _context;
        private readonly IMapper _mapper;

        public GenresController(GASPGDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            var Genres = await _context.Genres
                .Select(m => _mapper.Map<GenreViewModel>(m)).ToListAsync();
            return View(await _context.Genres.ToListAsync());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .SingleOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<DetailsGenreViewModel>(genre);
            return View(viewModel);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var genre = new Genre { Name = model.Name };
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.SingleOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EditGenreViewModel>(genre);
            return View(viewModel);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var genre = await _context.Genres
                    .SingleOrDefaultAsync(m => m.GenreId == model.GenreId);
                genre.Name = model.Name;
                _context.Update(genre);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(model.GenreId))
                {
                    return View("NotFound");
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var genre = await _context.Genres
                .SingleOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<DeleteGenreViewModel>(genre);
            return View(viewModel);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteGenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var genre = await _context.Genres
                .SingleOrDefaultAsync(m => m.GenreId == model.GenreId);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(string id)
        {
            return _context.Genres.Any(e => e.GenreId == id);
        }
    }
}
