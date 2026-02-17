using EchoCast.Domain.Entities;
using EchoCast.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EchoCast.Web.Controllers;

public class CommentsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CommentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Comments
    public async Task<IActionResult> Index()
    {
        var comments = await _context.Comments
            .Include(c => c.Episode)
            .Include(c => c.User)
            .ToListAsync();
        return View(comments);
    }

    // GET: Comments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var comment = await _context.Comments
            .Include(c => c.Episode)
            .Include(c => c.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (comment == null) return NotFound();

        return View(comment);
    }

    // GET: Comments/Create
    public IActionResult Create()
    {
        ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Name");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
        return View();
    }

    // POST: Comments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Content,EpisodeId,UserId")] Comment comment)
    {
        if (!ModelState.IsValid)
        {
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Name", comment.EpisodeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", comment.UserId);
            return View(comment);
        }

        _context.Add(comment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Comments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null) return NotFound();

        ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Name", comment.EpisodeId);
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", comment.UserId);
        return View(comment);
    }

    // POST: Comments/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Content,EpisodeId,UserId")] Comment comment)
    {
        if (id != comment.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["EpisodeId"] = new SelectList(_context.Episodes, "Id", "Name", comment.EpisodeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", comment.UserId);
            return View(comment);
        }

        try
        {
            _context.Update(comment);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(comment.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Comments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var comment = await _context.Comments
            .Include(c => c.Episode)
            .Include(c => c.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (comment == null) return NotFound();

        return View(comment);
    }

    // POST: Comments/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }
}

