using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApplicationTemp.Models;

namespace TodoApplicationTemp.Data
{
    public class TodoApplicationTempContext : DbContext
    {
        public TodoApplicationTempContext (DbContextOptions<TodoApplicationTempContext> options)
            : base(options)
        {
        }

        public DbSet<TodoApplicationTemp.Models.TodoItem> TodoItem { get; set; } = default!;
    }
}
