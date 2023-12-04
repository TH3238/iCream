using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ICream.Models;

namespace ICream.Data
{
    public class ICreamContext : DbContext
    {
        public ICreamContext (DbContextOptions<ICreamContext> options)
            : base(options)
        {
        }

        public DbSet<ICream.Models.ICreamList> ICreamList { get; set; } = default!;


        public DbSet<ICream.Models.OrderDetails> OrderDetails { get; set; } = default!;


        public DbSet<ICream.Models.ContactM> ContactM { get; set; } = default!;

        public DbSet<ICream.Models.LiveOrders> LiveOrders { get; set; } = default!;


    }
}
