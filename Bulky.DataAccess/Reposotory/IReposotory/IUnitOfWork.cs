﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Reposotory.IReposotory
{
    public interface IUnitOfWork
    { 
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        void Save();

    }
}
