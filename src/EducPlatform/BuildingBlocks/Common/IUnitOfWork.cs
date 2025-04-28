namespace BuildingBlocks.Common
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
