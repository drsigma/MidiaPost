using MidiaPost.CQRS.Core.Lib.Queries;

namespace MidiaPost.Query.Api.Queries
{
    public class FindPostByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}
