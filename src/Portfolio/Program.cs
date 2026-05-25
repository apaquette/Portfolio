using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portfolio;
using Repositories;
using Models.Career;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Core Infrastructure
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

// Repositories
builder.Services.AddScoped<IRepository<Award>, AwardRepository>();
builder.Services.AddScoped<IRepository<Certification>, CertificationRepository>();
builder.Services.AddScoped<IRepository<Degree>, DegreeRepository>();
builder.Services.AddScoped<ProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IRepository<SkillCategory>, SkillCategoryRepository>();
builder.Services.AddScoped<IRepository<Experience>, ExperienceRepository>();


// Feature Services
// Navigation Service
// PageComposition Service

// Filter Services
// ProjectFilterService
// ExperienceFilterService
// ProjectFilterFactory


// Configuration
// AppConstants

await builder.Build().RunAsync();
