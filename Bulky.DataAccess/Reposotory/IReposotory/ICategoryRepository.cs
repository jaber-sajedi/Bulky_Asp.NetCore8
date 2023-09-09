using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Reposotory.IReposotory
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category obj);
      
    }
}
