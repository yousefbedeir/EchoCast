using EchoCast.Domain.Entities;
using EchoCast.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EchoCast.Web.Controllers;

public class NotificationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public NotificationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Notifications
    public async Task<IActionResult> Index()
    {
        var notifications = await _context.Notifications
            .Include(n => n.User)
            .ToListAsync();
        return View(notifications);
    }

    // GET: Notifications/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var notification = await _context.Notifications
            .Include(n => n.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (notification == null) return NotFound();

        return View(notification);
    }

    // GET: Notifications/Create
    public IActionResult Create()
    {
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
        return View();
    }

    // POST: Notifications/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Message,CreatedAt,IsRead,UserId")] Notification notification)
    {
        if (!ModelState.IsValid)
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", notification.UserId);
            return View(notification);
        }

        _context.Add(notification);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Notifications/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null) return NotFound();

        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", notification.UserId);
        return View(notification);
    }

    // POST: Notifications/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Message,CreatedAt,IsRead,UserId")] Notification notification)
    {
        if (id != notification.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", notification.UserId);
            return View(notification);
        }

        try
        {
            _context.Update(notification);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NotificationExists(notification.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Notifications/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var notification = await _context.Notifications
            .Include(n => n.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (notification == null) return NotFound();

        return View(notification);
    }

    // POST: Notifications/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool NotificationExists(int id)
    {
        return _context.Notifications.Any(e => e.Id == id);
    }
}

