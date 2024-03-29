﻿using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        new Product? GetProduct(int id);

        new Product? Remove(Product item); 
    }
}