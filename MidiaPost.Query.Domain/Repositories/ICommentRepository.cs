using MidiaPost.Cmd.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentEntity comment);
        Task UpdateAsync(CommentEntity comment);
        Task DeleteAsync(Guid commentId);
        Task<CommentEntity> GetByIdAsync(Guid commentId);
    }
}
