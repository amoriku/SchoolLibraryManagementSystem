using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Infrastructure;

namespace SchoolLibrary.Application.Services
{
    public abstract class BaseService <T> where T : class
    {
        protected readonly AppDbContext context;
        protected readonly ILogger<T> logger;
        //protected readonly DbSet<T> dbSet;

        public BaseService(AppDbContext context, ILogger<T> logger)
        {
            this.context = context;
            this.logger = logger;
            //dbSet = context.Set<T>();
        }

        //public virtual async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken) => await dbSet.FindAsync(id, cancellationToken);
        //public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken) => await dbSet.ToListAsync(cancellationToken);
        //public virtual async Task SaveChangesAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken);
        //public virtual void Remove(T entity) => dbSet.Remove(entity);
    }
}
