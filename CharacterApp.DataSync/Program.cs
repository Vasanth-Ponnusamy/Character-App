using CharacterApp.Data.Context;
using CharacterApp.DataSync.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CharacterApp.Data;
using CharacterApp.Data.Repository;
using AutoMapper;
using System;

namespace CharacterApp.DataSync
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();

            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }));


            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<CharecterFetchHelper>();


            var serviceProvider = services.BuildServiceProvider();
            var helper = serviceProvider.GetRequiredService<CharecterFetchHelper>();

            await helper.SyncDataFromApiAsync();
        }        
    }
}
