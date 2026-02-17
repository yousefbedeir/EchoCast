using EchoCast.Domain.Entities;
using EchoCast.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EchoCast.Web.Controllers;

public class EpisodesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EpisodesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Episodes
    public async Task<IActionResult> Index()
    {
        var episodes = await _context.Episodes
            .Include(e => e.Podcast)
            .ToListAsync();
        return View(episodes);
    }

    // GET: Episodes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var episode = await _context.Episodes
            .Include(e => e.Podcast)
            .Include(e => e.Comments)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (episode == null) return NotFound();

        return View(episode);
    }

    // GET: Episodes/Create
    public IActionResult Create()
    {
        ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name");
        return View();
    }

    // POST: Episodes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl,PodcastId")] Episode episode)
    {
        if (!ModelState.IsValid)
        {
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name", episode.PodcastId);
            return View(episode);
        }

        _context.Add(episode);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Episodes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var episode = await _context.Episodes.FindAsync(id);
        if (episode == null) return NotFound();

        ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name", episode.PodcastId);
        return View(episode);
    }

    // POST: Episodes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,PodcastId")] Episode episode)
    {
        if (id != episode.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name", episode.PodcastId);
            return View(episode);
        }

        try
        {
            _context.Update(episode);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EpisodeExists(episode.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Episodes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var episode = await _context.Episodes
            .Include(e => e.Podcast)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (episode == null) return NotFound();

        return View(episode);
    }

    // POST: Episodes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var episode = await _context.Episodes.FindAsync(id);
        if (episode != null)
        {
            _context.Episodes.Remove(episode);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool EpisodeExists(int id)
    {
        return _context.Episodes.Any(e => e.Id == id);
    }
}

