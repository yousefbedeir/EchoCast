using EchoCast.Domain.Entities;
using EchoCast.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EchoCast.Web.Controllers;

public class PodcastsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PodcastsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Podcasts
    public async Task<IActionResult> Index()
    {
        var podcasts = await _context.Podcasts
            .Include(p => p.Creator)
            .ToListAsync();
        return View(podcasts);
    }

    // GET: Podcasts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var podcast = await _context.Podcasts
            .Include(p => p.Creator)
            .Include(p => p.Episodes)
            .Include(p => p.Ratings)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (podcast == null) return NotFound();

        return View(podcast);
    }

    // GET: Podcasts/Create
    public IActionResult Create()
    {
        ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Name");
        return View();
    }

    // POST: Podcasts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,About,ImageUrl,CreatorId")] Podcast podcast)
    {
        if (!ModelState.IsValid)
        {
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Name", podcast.CreatorId);
            return View(podcast);
        }

        _context.Add(podcast);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Podcasts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var podcast = await _context.Podcasts.FindAsync(id);
        if (podcast == null) return NotFound();

        ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Name", podcast.CreatorId);
        return View(podcast);
    }

    // POST: Podcasts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,About,ImageUrl,CreatorId")] Podcast podcast)
    {
        if (id != podcast.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Name", podcast.CreatorId);
            return View(podcast);
        }

        try
        {
            _context.Update(podcast);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PodcastExists(podcast.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Podcasts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var podcast = await _context.Podcasts
            .Include(p => p.Creator)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (podcast == null) return NotFound();

        return View(podcast);
    }

    // POST: Podcasts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var podcast = await _context.Podcasts.FindAsync(id);
        if (podcast != null)
        {
            _context.Podcasts.Remove(podcast);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PodcastExists(int id)
    {
        return _context.Podcasts.Any(e => e.Id == id);
    }
}

