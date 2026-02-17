using EchoCast.Domain.Entities;
using EchoCast.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EchoCast.Web.Controllers;

public class RolesController : Controller
{
    private readonly ApplicationDbContext _context;

    public RolesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Roles
    public async Task<IActionResult> Index()
    {
        var roles = await _context.Roles.ToListAsync();
        return View(roles);
    }

    // GET: Roles/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var role = await _context.Roles
            .FirstOrDefaultAsync(m => m.Id == id);
        if (role == null) return NotFound();

        return View(role);
    }

    // GET: Roles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Roles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] Role role)
    {
        if (!ModelState.IsValid) return View(role);

        _context.Add(role);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Roles/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var role = await _context.Roles.FindAsync(id);
        if (role == null) return NotFound();

        return View(role);
    }

    // POST: Roles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Role role)
    {
        if (id != role.Id) return NotFound();

        if (!ModelState.IsValid) return View(role);

        try
        {
            _context.Update(role);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoleExists(role.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Roles/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var role = await _context.Roles
            .FirstOrDefaultAsync(m => m.Id == id);
        if (role == null) return NotFound();

        return View(role);
    }

    // POST: Roles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool RoleExists(int id)
    {
        return _context.Roles.Any(e => e.Id == id);
    }
}

