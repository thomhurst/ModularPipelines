using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Helpers;

public static class Disposer
{
    public static async Task DisposeObjectAsync(object? obj)
    {
        if (obj is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }

        if (obj is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public static void DisposeObject(object? obj) => DisposeObjectAsync(obj).GetAwaiter().GetResult();
    
    [ExcludeFromCodeCoverage]
    public static void RegisterOnShutdown(object? obj)
    {
        Console.CancelKeyPress += (_, _) => DisposeObject(obj);

        AppDomain.CurrentDomain.ProcessExit += (_, _) => DisposeObject(obj);
    }
}