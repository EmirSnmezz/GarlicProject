using System.Linq.Expressions;

public interface IGenericRepository<TEntity>
{
    public void Add(TEntity entity);
    public void Delete(TEntity entity);
    public void Update(TEntity entity);
    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);
    public TEntity Get(Expression<Func<TEntity, bool>> filter);
}