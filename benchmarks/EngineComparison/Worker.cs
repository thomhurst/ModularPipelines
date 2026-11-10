using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines;
#if LEGACY
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
#endif

namespace EngineComparison;

internal static class Worker
{
    internal static int[] Completed = [];
    internal static string Shape = string.Empty;

    public static async Task Main(string[] args)
    {
        Shape = args[0];
        var count = int.Parse(args[1], System.Globalization.CultureInfo.InvariantCulture);
        var registrations = Scenarios.Registrations[Shape];
        if (count < 1 || count != registrations.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        var concreteModules = typeof(Worker).Assembly.GetTypes()
            .Count(type => !type.IsAbstract && typeof(TrivialModule).IsAssignableFrom(type));
        if (concreteModules != count)
        {
            throw new InvalidOperationException("The worker must contain exactly N concrete module types.");
        }

        // Warm the same generic registrations, dependency graph, JIT and command path.
        await RunPipelineAsync(registrations, count);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);
        var start = Stopwatch.GetTimestamp();
        var phases = await RunPipelineAsync(registrations, count);
        var elapsed = Stopwatch.GetElapsedTime(start);
        var allocated = GC.GetTotalAllocatedBytes(precise: true) - allocatedBefore;
        using var process = Process.GetCurrentProcess();
        process.Refresh();

        var result = new
        {
            Shape,
            Count = count,
            WallMilliseconds = elapsed.TotalMilliseconds,
            phases.BuildMilliseconds,
            phases.RunMilliseconds,
            phases.DisposeMilliseconds,
            AllocatedBytes = allocated,
            PeakWorkingSetBytes = process.PeakWorkingSet64,
            FrameworkAssembly = typeof(Pipeline).Assembly.GetName().Version?.ToString(),
            Runtime = RuntimeInformation.FrameworkDescription,
            OS = RuntimeInformation.OSDescription,
            Architecture = RuntimeInformation.ProcessArchitecture.ToString(),
            ProcessorCount = Environment.ProcessorCount,
            ServerGC = GCSettings.IsServerGC,
            Command = OperatingSystem.IsWindows() ? "cmd.exe /d /c exit 0" : "/bin/sh -c 'exit 0'",
            MaxParallelism = 8,
        };
        await File.WriteAllTextAsync(args[2], JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static async Task<PhaseTimings> RunPipelineAsync(Action<PipelineBuilder>[] registrations, int count)
    {
        var start = Stopwatch.GetTimestamp();
        Completed = new int[count];
#if LEGACY
        var builder = Pipeline.CreateBuilder(new PipelineBuilderOptions { ContentRootPath = AppContext.BaseDirectory });
        builder.Options.ShowProgressInConsole = false;
        builder.Options.PrintLogo = false;
        builder.Options.PrintResults = false;
        builder.Options.PrintDependencyChains = false;
        builder.Options.Concurrency.MaxParallelism = 8;
        builder.SetLogLevel(LogLevel.None);
#else
        var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
        {
            WorkingDirectory = AppContext.BaseDirectory,
            ContentRootPath = AppContext.BaseDirectory,
        });
        builder.ConfigureOptions(options => options with
        {
            Console = options.Console with
            {
                ShowProgress = false,
                PrintLogo = false,
                PrintResults = false,
                PrintDependencyChains = false,
            },
            Concurrency = options.Concurrency with { MaxParallelism = 8 },
            RunReport = options.RunReport with { HistoryRetention = 0, AutoWriteInCi = false },
        });
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.None);
#endif
        for (var i = 0; i < count; i++)
        {
            registrations[i](builder);
        }

        var pipeline = await builder.BuildAsync();
        var built = Stopwatch.GetTimestamp();
        long ran;
        try
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await pipeline.RunAsync(timeout.Token);
            ran = Stopwatch.GetTimestamp();
        }
        finally { await pipeline.DisposeAsync(); }
        var disposed = Stopwatch.GetTimestamp();

        if (Completed.Any(value => value != 1))
        {
            throw new InvalidOperationException("Every registered module must execute exactly once.");
        }

        return new PhaseTimings(
            Stopwatch.GetElapsedTime(start, built).TotalMilliseconds,
            Stopwatch.GetElapsedTime(built, ran).TotalMilliseconds,
            Stopwatch.GetElapsedTime(ran, disposed).TotalMilliseconds);
    }

    private readonly record struct PhaseTimings(double BuildMilliseconds, double RunMilliseconds, double DisposeMilliseconds);
}

public abstract class TrivialModule(int index, int predecessor) : Module<int>
{
    protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        if (predecessor >= 0 && Volatile.Read(ref Worker.Completed[predecessor]) != 1)
        {
            throw new InvalidOperationException("A dependency did not complete before its dependent.");
        }

        if (Worker.Shape == "commands")
        {
            var tool = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/sh";
            string[] arguments = OperatingSystem.IsWindows() ? ["/d", "/c", "exit", "0"] : ["-c", "exit 0"];
#if LEGACY
            var result = await context.Shell.Command.ExecuteCommandLineTool(
                new RawCommandOptions { Tool = tool, Arguments = arguments },
                cancellationToken: cancellationToken);
#else
            var result = await context.Shell.RunAsync(tool, arguments, cancellationToken);
#endif
            if (result.ExitCode != 0)
            {
                throw new InvalidOperationException($"Command failed: {result.ExitCode}");
            }
        }

        if (Interlocked.Increment(ref Worker.Completed[index]) != 1)
        {
            throw new InvalidOperationException("A module executed more than once.");
        }

        return index;
    }
}

#if LEGACY
internal sealed record RawCommandOptions : CommandLineToolOptions;
#endif
