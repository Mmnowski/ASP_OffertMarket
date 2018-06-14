using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GASPG.Data;
using GASPG.Models;
using AutoMapper;
using GASPG.Models.ViewModel.DeveloperViewModels;

namespace GASPG.Controllers
{
    public class DevelopersController : Controller
    {
        private readonly GASPGDbContext _context;
        private readonly IMapper _mapper;

        public DevelopersController(GASPGDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Developers
        public async Task<IActionResult> Index()
        {
            var developers = await _context.Developers
                .Select(m => _mapper.Map<DeveloperViewModel>(m)).ToListAsync();
            return View(developers);
        }

        // GET: JobType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var developer = await _context.Developers
                .SingleOrDefaultAsync(m => m.DeveloperId == id);
            if (developer == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<DetailsDeveloperViewModel>(developer);
            return View(viewModel);
        }

        // GET: JobType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDeveloperViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var developer = new Developer { Name = model.Name };
            _context.Add(developer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: JobType/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var developer = await _context.Developers
                .SingleOrDefaultAsync(m => m.DeveloperId == id);
            if (developer == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<EditDeveloperViewModel>(developer);
            return View(viewModel);
        }

        // POST: JobType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDeveloperViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var developer = await _context.Developers
                    .SingleOrDefaultAsync(m => m.DeveloperId == model.DeveloperId);
                developer.Name = model.Name;
                _context.Update(developer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeveloperExists(model.DeveloperId))
                {
                    return View("NotFound");
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobType/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var developer = await _context.Developers
                .SingleOrDefaultAsync(m => m.DeveloperId == id);
            if (developer == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<DeleteDeveloperViewModel>(developer);
            return View(viewModel);
        }

        // POST: JobType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteDeveloperViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var developer = await _context.Developers
                .SingleOrDefaultAsync(m => m.DeveloperId == model.DeveloperId);
            _context.Developers.Remove(developer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeveloperExists(string id)
        {
            return _context.Developers.Any(e => e.DeveloperId == id);
        }
    }
}
