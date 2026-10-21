using System.Text.Json;
using ModularPipelines;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Redis;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.SmokeTests;
using ModularPipelines.GitHub;
using YamlDotNet.RepresentationModel;

const string project = "test/ModularPipelines.Distributed.SmokeTests/ModularPipelines.Distributed.SmokeTests.csproj";

if (args is ["--identity", var identityPath])
{
    var registry = new ModuleTypeRegistry();
    registry.Register(typeof(WindowsProbeModule));
    var identity = new
    {
        Schema = registry.GetPipelineSchemaVersion(),
        Mvid = typeof(WindowsProbeModule).Module.ModuleVersionId,
        OperatingSystem = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
        AssemblyPath = typeof(WindowsProbeModule).Assembly.Location,
    };
    var json = JsonSerializer.Serialize(identity, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(identityPath, json);
    Console.WriteLine(json);
    return;
}

var builder = Pipeline.CreateBuilder();
builder.AddModule<WindowsProbeModule>();

if (args is ["--generate", var commandDirectory])
{
    WindowsProbeModule.GeneratingWorkflow = true;
    Directory.CreateDirectory(commandDirectory);
    var workflowPath = Path.Combine(commandDirectory, "workflow.yml");
    builder.AddDistributedWorkflow(new DistributedWorkflowOptions
    {
        PipelineProjectPath = new(project),
        DotNetRunFramework = "net11.0",
        ExtraWorkers = 0,
        CacheNuGet = false,
        OutputPath = new(workflowPath),
    });
    await builder.RunAsync();

    // Execute the writer's actual commands in CI, so the smoke check cannot drift from the template.
    var yaml = new YamlStream();
    yaml.Load(new StringReader(await File.ReadAllTextAsync(workflowPath)));
    var jobs = (YamlMappingNode) yaml.Documents[0].RootNode["jobs"];
    foreach (var (job, stepName, script) in new[]
             {
                 ("initialize", "Publish portable pipeline host", "publish.sh"),
                 ("pipeline", "Restore runner dependencies", "restore.sh"),
                 ("pipeline", "Run Pipeline", "run.sh"),
             })
    {
        var steps = (YamlSequenceNode) jobs[job]["steps"];
        var step = steps.Children.Cast<YamlMappingNode>().Single(node => node["name"].ToString() == stepName);
        await File.WriteAllTextAsync(Path.Combine(commandDirectory, script), "set -euo pipefail\n" + step["run"] + "\n");
        if (script == "run.sh")
        {
            await File.WriteAllTextAsync(Path.Combine(commandDirectory, "run-identity.sh"),
                "set -euo pipefail\n" + step["run"] + " -- --identity \"$RUNNER_TEMP/identities/shared-$RUNNER_OS.json\"\n");
        }
    }

    return;
}

builder.AddDistributedMode();
builder.ConfigureDistributedOptions(options => options with
{
    MinimumWorkerCount = 1,
    WorkerRegistrationTimeout = TimeSpan.FromMinutes(5),
    ModuleResultTimeout = TimeSpan.FromMinutes(1),
});
builder.AddRedisDistributedCoordinator(options => options with
{
    ConnectionString = Environment.GetEnvironmentVariable("REDIS_ENDPOINT")
        ?? throw new InvalidOperationException("REDIS_ENDPOINT is required."),
    ConfigureConnection = connection => connection with
    {
        Password = Environment.GetEnvironmentVariable("REDIS_KEY"),
        UseTls = true,
    },
    KeyPrefix = "modularpipelines-template-smoke",
    TimeToLive = TimeSpan.FromMinutes(10),
});

var summary = await builder.RunAsync();
if (Environment.GetEnvironmentVariable("MODULARPIPELINES_INSTANCE_INDEX") == "0")
{
    if (summary.GetResult<WindowsProbeModule, string>() is not ModuleResult<string>.Success { Value: "windows-ok" })
    {
        throw new InvalidOperationException("The master did not receive the Windows worker's successful result.");
    }

    Console.WriteLine("Cross-OS worker registration and module execution passed.");
}
