namespace Application.Extensions;

public static class QueryablePageFilterExtension
{
    public static IQueryable<T> ExecutePageFilter<T>(this IQueryable<T> queryable, int page, int take)
        where T : class
    {
        return queryable.Skip((page > 0 ? page - 1 : 0) * take).Take(take);
    }
}