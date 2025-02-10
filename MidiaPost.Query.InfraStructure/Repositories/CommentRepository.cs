using Microsoft.EntityFrameworkCore;
using MidiaPost.Cmd.Domain.Entities;
using MidiaPost.Query.Domain.Repositories;
using MidiaPost.Query.InfraStructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiaPost.Query.InfraStructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task CreateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Add(comment);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);

            if (comment != null) return;

            context.Comments.Remove(comment);

            _ = await context.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Comments
                    .FirstOrDefaultAsync(x => x.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Update(comment);

            _ = await context.SaveChangesAsync();
        }
    }
}
