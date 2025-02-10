using MidiaPost.Cmd.Domain.Entities;
using MidiaPost.CQRS.Core.Lib.InfraStructure;
using MidiaPost.CQRS.Core.Lib.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.Query.InfraStructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<PostEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handlers = new();
        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException("Cannot Register");
            }
            _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public Task<List<PostEntity>> SendAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<PostEntity>>> handler))
            {
                return handler(query);
            }
            throw new ArgumentNullException(nameof(handler));
        }
    }
}
