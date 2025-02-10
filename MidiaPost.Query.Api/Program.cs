
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using MidiaPost.Cmd.Domain.Entities;
using MidiaPost.CQRS.Core.Lib.Consumers;
using MidiaPost.CQRS.Core.Lib.InfraStructure;
using MidiaPost.Query.Api.Queries;
using MidiaPost.Query.Domain.Repositories;
using MidiaPost.Query.InfraStructure.Consumers;
using MidiaPost.Query.InfraStructure.DataAccess;
using MidiaPost.Query.InfraStructure.Dispatchers;
using MidiaPost.Query.InfraStructure.Handlers;
using MidiaPost.Query.InfraStructure.Repositories;

namespace MidiaPost.Query.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Action<DbContextOptionsBuilder> configureDbContext =
                o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
            builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));  

            var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
            dataContext.Database.EnsureCreated();

            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IQueryHandler, QueryHandler>();
            builder.Services.AddScoped<IEventHandler, InfraStructure.Handlers.EventHandler>();
            builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
            builder.Services.AddScoped<IEventConsumer, EventConsumer>();

            // register query handler methods
            var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
            var dispatcher = new QueryDispatcher();
            dispatcher.RegisterHandler<FindAllPostsQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync); 
            dispatcher.RegisterHandler<FindPostsByAuthorQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostsWithCommentsQuery>(queryHandler.HandleAsync);
            dispatcher.RegisterHandler<FindPostsWithLikesQuery>(queryHandler.HandleAsync);
            builder.Services.AddSingleton<IQueryDispatcher<PostEntity>>(_ => dispatcher);

            builder.Services.AddControllers();
            builder.Services.AddHostedService<ConsumerHostedService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
