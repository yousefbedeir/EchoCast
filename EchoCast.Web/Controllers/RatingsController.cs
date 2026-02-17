using EchoCast.Domain.Entities;
using EchoCast.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EchoCast.Web.Controllers;

public class RatingsController : Controller
{
    private readonly ApplicationDbContext _context;

    public RatingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Ratings
    public async Task<IActionResult> Index()
    {
        var ratings = await _context.Ratings
            .Include(r => r.Podcast)
            .Include(r => r.User)
            .ToListAsync();
        return View(ratings);
    }

    // GET: Ratings/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var rating = await _context.Ratings
            .Include(r => r.Podcast)
            .Include(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (rating == null) return NotFound();

        return View(rating);
    }

    // GET: Ratings/Create
    public IActionResult Create()
    {
        ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
        return View();
    }

    // POST: Ratings/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Stars,PodcastId,UserId")] Rating rating)
    {
        if (!ModelState.IsValid)
        {
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name", rating.PodcastId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", rating.UserId);
            return View(rating);
        }

        _context.Add(rating);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Ratings/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var rating = await _context.Ratings.FindAsync(id);
        if (rating == null) return NotFound();

        ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name", rating.PodcastId);
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", rating.UserId);
        return View(rating);
    }

    // POST: Ratings/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Stars,PodcastId,UserId")] Rating rating)
    {
        if (id != rating.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["PodcastId"] = new SelectList(_context.Podcasts, "Id", "Name", rating.PodcastId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", rating.UserId);
            return View(rating);
        }

        try
        {
            _context.Update(rating);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RatingExists(rating.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Ratings/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var rating = await _context.Ratings
            .Include(r => r.Podcast)
            .Include(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (rating == null) return NotFound();

        return View(rating);
    }

    // POST: Ratings/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var rating = await _context.Ratings.FindAsync(id);
        if (rating != null)
        {
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool RatingExists(int id)
    {
        return _context.Ratings.Any(e => e.Id == id);
    }
}

