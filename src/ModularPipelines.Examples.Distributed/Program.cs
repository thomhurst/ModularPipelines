using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Options;
using ModularPipelines.Examples.Distributed.Modules;
using ModularPipelines.Host;

namespace ModularPipelines.Examples.Distributed;

/// <summary>
/// Example demonstrating distributed ModularPipelines execution.
/// Can run in orchestrator or worker mode based on command-line arguments.
/// </summary>
public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Console.WriteLine("ModularPipelines Distributed Execution Example");
        Console.WriteLine("===============================================");
        Console.WriteLine();

        // Parse command-line arguments
        var mode = args.Length > 0 ? args[0].ToLowerInvariant() : "help";

        try
        {
            return mode switch
            {
                "orchestrator" => await RunOrchestratorAsync(args),
                "worker" => await RunWorkerAsync(args),
                _ => ShowHelp()
            };
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Fatal error: {ex.Message}");
            Console.ResetColor();
            return 1;
        }
    }

    private static async Task<int> RunOrchestratorAsync(string[] args)
    {
        Console.WriteLine("🎯 Starting in ORCHESTRATOR mode");
        Console.WriteLine();

        var port = args.Length > 1 && int.TryParse(args[1], out var p) ? p : 8080;

        var summary = await PipelineHostBuilder.Create()
            .AddDistributedExecution(options =>
            {
                options.Mode = DistributedExecutionMode.Orchestrator;
                options.OrchestratorPort = port;
                options.WorkerHeartbeatTimeout = TimeSpan.FromMinutes(2);
                options.WorkerHeartbeatInterval = TimeSpan.FromSeconds(30);
                options.MaxRetryAttempts = 3;
                options.EnableCompression = true;
                options.PreferLocalExecution = true;
            })
            .AsOrchestrator(port)
            .AddModule<FetchDataModule>()
            .AddModule<ValidateEnvironmentModule>()
            .AddModule<ProcessDataModule>()
            .AddModule<PublishResultsModule>()
            .ExecutePipelineAsync();

        Console.WriteLine();
        Console.WriteLine("📊 Pipeline Summary");
        Console.WriteLine($"   Status: {summary.Status}");
        Console.WriteLine($"   Duration: {summary.TotalDuration}");
        Console.WriteLine($"   Modules: {summary.Modules.Count}");

        return summary.Status == ModularPipelines.Enums.Status.Successful ? 0 : 1;
    }

    private static async Task<int> RunWorkerAsync(string[] args)
    {
        Console.WriteLine("⚙️ Starting in WORKER mode");
        Console.WriteLine();

        var orchestratorUrl = args.Length > 1 ? args[1] : "http://localhost:8080";
        var workerId = args.Length > 2 ? args[2] : null;
        var workerPort = args.Length > 3 && int.TryParse(args[3], out var p) ? p : 9000;

        Console.WriteLine($"   Orchestrator: {orchestratorUrl}");
        Console.WriteLine($"   Worker ID: {workerId ?? "(auto-generated)"}");
        Console.WriteLine($"   Worker Port: {workerPort}");
        Console.WriteLine();

        var capabilities = new WorkerCapabilities
        {
            // Os is automatically detected by WorkerCapabilities.DetectCurrentOs()
            InstalledTools = new List<string> { "dotnet", "git" },
            MaxParallelModules = Environment.ProcessorCount,
            Tags = new List<string> { "example-worker" },
        };

        await PipelineHostBuilder.Create()
            .AddDistributedExecution()
            .ConfigureServices((context, services) =>
            {
                services.Configure<DistributedPipelineOptions>(options =>
                {
                    options.Mode = DistributedExecutionMode.Worker;
                    options.OrchestratorUrl = orchestratorUrl;
                    options.WorkerCapabilities = capabilities;

                    if (!string.IsNullOrWhiteSpace(workerId))
                    {
                        options.WorkerId = workerId;
                    }

                    options.WorkerPort = workerPort;
                });
            })
            .AddModule<FetchDataModule>()
            .AddModule<ValidateEnvironmentModule>()
            .AddModule<ProcessDataModule>()
            .AddModule<PublishResultsModule>()
            .RunWorkerAsync();

        return 0;
    }

    private static int ShowHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- orchestrator [port]");
        Console.WriteLine("  dotnet run -- worker [orchestrator-url] [worker-id] [worker-port]");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  dotnet run -- orchestrator 8080");
        Console.WriteLine("  dotnet run -- worker http://localhost:8080 worker1 9000");
        Console.WriteLine("  dotnet run -- worker http://localhost:8080 worker2 9001");
        Console.WriteLine();
        Console.WriteLine("Default values:");
        Console.WriteLine("  Orchestrator port: 8080");
        Console.WriteLine("  Orchestrator URL: http://localhost:8080");
        Console.WriteLine("  Worker ID: auto-generated");
        Console.WriteLine("  Worker port: 9000");
        Console.WriteLine();

        return 0;
    }
}
