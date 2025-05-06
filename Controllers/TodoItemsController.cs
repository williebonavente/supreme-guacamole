using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TodoApplicationTemp.Data;
using TodoApplicationTemp.Models;

namespace TodoApplicationTemp.Controllers
{
    public class TodoItemsController : Controller
    {
        // Database context automatically injected by ASP.NET Core
        private readonly TodoApplicationTempContext _context;

        public TodoItemsController(TodoApplicationTempContext context)
        {
            _context = context;
        }

        // GET: TodoItems
        public IActionResult Index(string category, int? priority, string sortOrder)
        {
            var items = _context.TodoItem.AsQueryable();

            if (!String.IsNullOrEmpty(category))
            {
                items = items.Where(t => t.Category == category);
            }

            if (priority.HasValue)
            {
                items = items.Where(t => t.Priority == priority.Value);
            }

            ViewBag.SortOrder = sortOrder;
            ViewBag.Categories = _context.TodoItem.Select(t => t.Category).Distinct().ToList();

            switch (sortOrder)
            {
                case "priority_desc":
                    items = items.OrderByDescending(t => t.Priority);
                    break;

                case "priority_asc":
                    items = items.OrderBy(t => t.Priority);
                    break;

                default:
                    items = items.OrderBy(t => t.Id);
                    break;
            }

            return View(items.ToList());
        }

        // GET: TodoItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // GET: TodoItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> Create([Bind("Id,Title,IsComplete,Category,Priority,DueDate,IsDeleted")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: TodoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return View(todoItem);
        }

        // POST: TodoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsComplete,Category,Priority,DueDate,IsDeleted")] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(todoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: TodoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: TodoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem != null)
            {
                _context.TodoItem.Remove(todoItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItem.Any(e => e.Id == id);
        }
        
        // Custom actions for TodoItem
        // POST: TodoItems/MarkComplete/5
        [HttpPost]
        public ActionResult MarkComplete(int id)
        {
            var item = _context.TodoItem.Find(id);
            if (item != null && item.IsComplete == false)
            {
                item.IsComplete = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: TodoItems/SoftDelete/5
        public ActionResult SoftDelete(int id)
        {
            var item = _context.TodoItem.Find(id);
            if (item != null)
            {
                _context.SaveChanges();
            }

            return RedirectToAction("index");
        }
    }
}
