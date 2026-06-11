namespace League_of_Legends_Tournament_Hosting.Controllers.Api
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var safePage = page < 1 ? 1 : page;
            var safePageSize = pageSize < 1 ? 20 : Math.Min(pageSize, 100);

            return query.Skip((safePage - 1) * safePageSize).Take(safePageSize);
        }
    }
}
