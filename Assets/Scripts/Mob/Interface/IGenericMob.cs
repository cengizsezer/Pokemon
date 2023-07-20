using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public interface IGenericMob<TEntity> where TEntity : PoolObject
{
    public TEntity GetMob(int index);
    public TEntity GetMob(Expression<Func<TEntity, bool>> predicate);
    public List<TEntity> GetElements(Expression<Func<TEntity,bool>> predicate);

    public void RemoveMobList(TEntity mob);

}
