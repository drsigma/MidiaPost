using MidiaPost.Cmd.Domain.Entities;
using MidiaPost.Common.DTOs;

namespace MidiaPost.Query.Api.DTOs
{
    public class PostLookupResponse : BaseResponse
    {
        public List<PostEntity> Posts { get; set; }
    }
}
