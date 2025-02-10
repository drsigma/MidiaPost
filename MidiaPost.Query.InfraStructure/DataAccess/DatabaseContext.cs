using Microsoft.EntityFrameworkCore;
using MidiaPost.Cmd.Domain.Entities;


namespace MidiaPost.Query.InfraStructure.DataAccess
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }  
        
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
    }
}
