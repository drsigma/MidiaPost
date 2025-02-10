using MidiaPost.CQRS.Core.Lib.Queries;

namespace MidiaPost.Query.Api.Queries
{
    public class FindPostsWithLikesQuery : BaseQuery
    {
        public int NumberOfLikes { get; set; }
    }
}
