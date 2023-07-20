using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public abstract class GenericMobController<TEntity> : IGenericMob<TEntity> where TEntity : PoolObject
{
    public List<TEntity> lsMobs = new();

    public TEntity GetMob(int index)
    {
        return lsMobs[index];
    }

    public TEntity GetMob(Expression<Func<TEntity, bool>> predicate)
    {
        var compiledPredicate = predicate.Compile();
        return lsMobs.FirstOrDefault(compiledPredicate);
    }

    public List<TEntity> GetElements(Expression<Func<TEntity, bool>> predicate)
    {
        var compiledPredicate = predicate.Compile();
        return lsMobs.Where(compiledPredicate).ToList();
    }

    public void RemoveMobList(TEntity mob)
    {
        if (lsMobs.Contains(mob))
        {
            lsMobs.Remove(mob);
        }
    }
}
