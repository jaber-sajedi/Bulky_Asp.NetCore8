﻿using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Reposotory.IReposotory
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product obj);
    }
}
