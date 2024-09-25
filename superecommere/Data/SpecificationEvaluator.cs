using superecommere.Models.Products;
using superecommere.Repositories.Interface;

namespace superecommere.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuary(IQueryable<T> query,
            ISpecification<T> spec) 
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);//x=>x.Brand==brand
            }
            if (spec.OrderBy != null) { 
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescendig != null)
            {
                query = query.OrderByDescending(spec.OrderByDescendig);
            }

            if (spec.IsDistinct)
            {
                query=query.Distinct();
            }
            if (spec.IsPagingEnabled)
            {
                query=query.Skip(spec.Skip).Take(spec.Take);
            }
            return query;
        }

        public static IQueryable<TResult> GetQuary<TSpec, TResult>(IQueryable<T> query,
            ISpecification<T, TResult> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);//x=>x.Brand==brand
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescendig != null)
            {
                query = query.OrderByDescending(spec.OrderByDescendig);
            }
            var selectQuary = query as IQueryable<TResult>;
            if (spec.Select != null)
            {
                selectQuary = query.Select(spec.Select);
            }
            if (spec.IsDistinct)
            {
                selectQuary = selectQuary?.Distinct();
            }
            if (spec.IsPagingEnabled)
            {
                selectQuary = selectQuary?.Skip(spec.Skip).Take(spec.Take);
            }
            return selectQuary ?? query.Cast<TResult>();
        }
    }
}
