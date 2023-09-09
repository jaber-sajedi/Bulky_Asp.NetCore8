using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Reposotory.IReposotory;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Reposotory
{
    public class CategoryRepository:Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

   

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
