using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class, new()
{
    public AppDbContext _context;
    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(TEntity entity)
    {
        EntityEntry addedEntity = _context.Set<TEntity>().Entry(entity);
        EntityState state = EntityState.Added;
        addedEntity.State = state;
        _context.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        EntityEntry deletedEntity = _context.Set<TEntity>().Entry(entity);
        EntityState state = EntityState.Deleted;
        deletedEntity.State = state;
        _context.SaveChanges();
    }

    public TEntity Get(Expression<Func<TEntity, bool>> filter)
    {
        TEntity result = _context.Set<TEntity>().FirstOrDefault(filter);

        if(result is not null)
        {
            return result;
        }

        return null;
    }

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> result;
        
        if(filter is not null)
        {
         result = _context.Set<TEntity>().Where(filter);   
         return result;
        }

        result = _context.Set<TEntity>();

         if(includes != null)
        {
            foreach(var include in includes)
            {
                result = result.Include(include);
            }
        }

        return result;
    }

    public void Update(TEntity entity)
    {
        EntityEntry updatedEntity = _context.Set<TEntity>().Entry(entity);
        EntityState state = EntityState.Modified;

        updatedEntity.State = state;

        _context.SaveChanges();
    }
}