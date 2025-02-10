using MidiaPost.CQRS.Core.Lib.Queries;

namespace MidiaPost.Query.Api.Queries
{
    public class FindPostsByAuthorQuery : BaseQuery
    {
        public string Author { get; set; }
    }
}
