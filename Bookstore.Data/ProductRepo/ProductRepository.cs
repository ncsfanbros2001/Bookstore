using Bookstore.Data.Repository;
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data.CategoryRepo
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly DatabaseContext _db;

        public ProductRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var objFromDB = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            if (objFromDB != null)
            {
                objFromDB.Title = product.Title;
                objFromDB.ISBN = product.ISBN;
                objFromDB.Description = product.Description;
                objFromDB.Price = product.Price;
                objFromDB.Price50 = product.Price50;
                objFromDB.Price100 = product.Price100;
                objFromDB.ListPrice = product.ListPrice;
                objFromDB.Author = product.Author;
                objFromDB.CategoryId = product.CategoryId;
                objFromDB.CoverTypeId = product.CoverTypeId;
                if (objFromDB.ImageURL != null)
                {
                    objFromDB.ImageURL = product.ImageURL;
                }
            }
        }
    }
}
