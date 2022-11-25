using CV.Application.Commands;
using CV.Application.Services;
using CV.Domain.Rooms.Interfaces;
using System.Reflection;

namespace CV.AppStart;

public static class IoC
{
    public static async Task RegisterAllCommands(this IServiceCollection services)
    {
        services.Scan(s =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                s.FromAssemblies(assemblies)
                    .AddClasses(c => c.AssignableTo(typeof(ICommand)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            }
        );
    }

    public static async Task RegisterAllRooms(this IServiceCollection services)
    {
        services.Scan(s =>
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
            var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IRoom)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        }
        );
    }

    public static async Task InitializeRooms(this IServiceProvider serviceProvider)
    {
        var rooms = serviceProvider.GetServices<IRoom>();
        var roomSvc = serviceProvider.GetService<IRoomsService>();
        foreach (var room in rooms)
        {
            await room.InitializeRoom();
            await roomSvc.RegisterRoom(room);
        }
    }
}