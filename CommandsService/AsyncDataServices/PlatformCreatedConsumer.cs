using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.EventProcessing;
using CommandsService.Models;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using SharedModels;
using System.Text.Json;

namespace CommandsService.AsyncDataServices
{
    public class PlatformCreatedConsumer : IConsumer<PlatformCreated>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PlatformCreatedConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Consume(ConsumeContext<PlatformCreated> context)
        {
            var jsonMessage = JsonSerializer.Serialize(context.Message);
            Console.WriteLine($"PlatformCreated message: {jsonMessage}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                try
                {
                    var plat = JsonSerializer.Deserialize<Platform>(jsonMessage);
                    plat.ExternalID = plat.Id;
                    if (!repo.ExternalPlatformExists(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Platform added");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exists...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"-->Could not add Platform to DB {ex.Message}");
                }
            }
        }
    }

}
