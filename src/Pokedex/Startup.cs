using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pokedex.Domain;
using Pokedex.Domain.Queries;
using Pokedex.Queries;
using Refit;

namespace Pokedex
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<BasicPokemonInformationRetriever>();
            services.AddTransient<TranslatedPokemonInformationRetriever>();
            services.AddTransient<IPokemonQuery, PokemonQuery>();
            services.AddTransient<ITranslationQuery, TranslationQuery>();
            services.AddSingleton<ICache<PokemonSpecies>>(sp => new FileCache<PokemonSpecies>("cache", sp.GetRequiredService<ILogger<FileCache<PokemonSpecies>>>()));
            services.AddSingleton<ICache<string>>(sp => new FileCache<string>("translation-cache", sp.GetRequiredService<ILogger<FileCache<string>>>()));

            services
                .AddRefitClient<IPokeApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://pokeapi.co/api/v2"));

            services
                .AddRefitClient<IFunTranslationsApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.funtranslations.com"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokedex", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex v1"));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
