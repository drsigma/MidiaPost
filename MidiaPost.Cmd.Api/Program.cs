
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MidiaPost.Cmd.Api.CommandHandlers;
using MidiaPost.Cmd.Api.Commands;
using MidiaPost.Cmd.Api.Producers;
using MidiaPost.Cmd.Domain.Aggregates;
using MidiaPost.Cmd.InfraStructure.Config;
using MidiaPost.Cmd.InfraStructure.Handlers;
using MidiaPost.Cmd.InfraStructure.NovaPasta;
using MidiaPost.Cmd.InfraStructure.Repository;
using MidiaPost.Cmd.InfraStructure.Stores;
using MidiaPost.CQRS.Core.Lib.Domain;
using MidiaPost.CQRS.Core.Lib.Handlers;
using MidiaPost.CQRS.Core.Lib.InfraStructure;
using MidiaPost.CQRS.Core.Lib.Producers;

namespace MidiaPost.Cmd.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));
            builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection("ProducerConfig"));
            builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            builder.Services.AddScoped<IEventProducer, EventProducer>();
            builder.Services.AddScoped<IEventStore, EventStore>();
            builder.Services.AddScoped<IEventSourcingHandlers<PostAggregate>, EventSourcingHandler>();
            builder.Services.AddScoped<ICommandHandler, CommandHandler>();

            var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
            var dispatcher = new CommandDispatcher();
            dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RestoreReadDbCommand>(commandHandler.HandleAsync);
            builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);



            builder.Services.AddControllers();
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
