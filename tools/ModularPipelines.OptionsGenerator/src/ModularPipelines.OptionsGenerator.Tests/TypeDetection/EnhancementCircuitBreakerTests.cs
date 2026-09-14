using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class EnhancementCircuitBreakerTests
{
    [Test]
    public async Task Enhancement_Probes_Configured_Process_After_Scraper_Circuit_Opens()
    {
        var process = new RecoveringProcessExecutor();
        var scraper = new ResilientCliCommandExecutor(process,
            NullLogger<ResilientCliCommandExecutor>.Instance,
            maxRetries: 0, baseDelay: TimeSpan.Zero,
            circuitBreakerThreshold: 5, circuitBreakerDuration: TimeSpan.FromMinutes(1));
        using var services = new ServiceCollection()
            .AddSingleton<ProcessCliCommandExecutor>(process)
            .AddSingleton<ICliCommandExecutor>(scraper)
            .AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance)
            .BuildServiceProvider();
        var enhancer = OptionsGeneratorCommand.CreateTypeEnhancer(services);

        for (var attempt = 0; attempt < 5; attempt++)
        {
            await scraper.ExecuteAsync("kubectl", "example --help");
        }

        await Assert.That((await scraper.ExecuteAsync("kubectl", "example --help")).CircuitOpen).IsTrue();
        process.Fail = false;
        var tool = new CliToolDefinition
        {
            ToolName = "kubectl",
            NamespacePrefix = "Kubectl",
            TargetNamespace = "ModularPipelines.Kubernetes",
            OutputDirectory = "unused",
            Commands =
            [
                new CliCommandDefinition
                {
                    FullCommand = "kubectl example",
                    CommandParts = ["example"],
                    ClassName = "KubectlExampleOptions",
                    ParentClassName = "KubectlOptions",
                    ToolNamespacePrefix = "Kubectl",
                    Options = [new CliOptionDefinition { SwitchName = "--style", PropertyName = "Style", CSharpType = "string?" }],
                },
            ],
        };

        var enhanced = await enhancer.EnhanceAsync(tool);
        var option = enhanced.Commands.Single().Options.Single();
        await Assert.That(option.EnumDefinition).IsNotNull();
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["compact", "expanded"]);
        await Assert.That(process.Calls).IsEqualTo(6);
        await Assert.That((await scraper.ExecuteAsync("kubectl", "example --help")).CircuitOpen).IsTrue();
    }

    // Reimplement the interface to exercise DI's configured process registration without launching a CLI.
    private sealed class RecoveringProcessExecutor()
        : ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance), ICliCommandExecutor
    {
        public bool Fail { get; set; } = true;

        public int Calls { get; private set; }

        Task<bool> ICliCommandExecutor.IsAvailableAsync(string command, CancellationToken cancellationToken) =>
            Task.FromResult(true);

        Task<CliCommandResult> ICliCommandExecutor.ExecuteAsync(
            string command, string arguments, CancellationToken cancellationToken, string? workingDirectory)
        {
            Calls++;
            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = Fail ? string.Empty : "  --style string   One of: compact|expanded",
                StandardError = string.Empty,
                ExitCode = Fail ? -1 : 0,
                TimedOut = Fail,
            });
        }
    }
}
