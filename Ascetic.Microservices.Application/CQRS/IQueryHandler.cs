namespace Ascetic.Microservices.Application.CQRS
{
    public interface IQueryHandler<TIn, TOut> : IHandler<TIn, TOut>
        where TIn : IQuery<TOut>
    {
    }
}
