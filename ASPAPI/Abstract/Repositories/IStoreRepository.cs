﻿using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
       new Store? GetById(int id);
    }
}
