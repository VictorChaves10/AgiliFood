namespace AgiliFood.Business.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Dispose();

        IProductRepository ProductRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
    }
}
