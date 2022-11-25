using CV.Application.Interfaces;
using CV.Application.Services;
using CV.AppStart;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CV.Infrastructure.Services;
using CV;
using CV.Application.Factories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IStateService, StateService>();

builder.Services.AddScoped<ICommandParserService, CommandParserService>();
builder.Services.AddScoped<ICommandHandlerFactory, CommandHandlerFactory>();
builder.Services.AddScoped<ITextDownloaderService, TextDownloaderService>();
builder.Services.AddScoped<IRoomsService, RoomsService>();

await builder.Services.RegisterAllRooms();
await builder.Services.RegisterAllCommands();

var svc = builder.Build();
await svc.Services.InitializeRooms();

await svc.RunAsync();
