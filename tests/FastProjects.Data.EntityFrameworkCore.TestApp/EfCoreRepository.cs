using FastProjects.SharedKernel;

namespace FastProjects.Data.EntityFrameworkCore.TestApp;

public class EfCoreRepository<T>(AppDbContext appDbContext)
    : EfCoreRepositoryBase<T>(appDbContext)
    where T : class, IAggregateRoot;
