using Bookstore.Data.Repository;
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data.CategoryRepo
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly DatabaseContext _db;

        public CategoryRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
