using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    private static readonly string[] KubectlResourceUpdates = ["owner=team"];

    [Test]
    [Arguments("annotate")]
    [Arguments("label")]
    public async Task Kubectl_Resource_Alternatives_Require_A_Complete_Target(string commandName)
    {
        var help = $$"""
            Usage:
              kubectl {{commandName}} (-f FILENAME | TYPE NAME) KEY_1=VAL_1 ... KEY_N=VAL_N [options]

            Options:
              -f, --filename stringArray   Files identifying resources.
              -k, --kustomize string       Kustomization directory.
                  --all=false: Select all resources.
              -l, --selector string       Label selector.
                  --field-selector string  Field selector.
            """;
        var scraper = new KubectlCliScraper(new ResourceFixtureExecutor(commandName, help),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<KubectlCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var definition in scraper.ScrapeAsync())
        {
            commands.Add(definition);
        }
        var command = commands.Single();
        var options = Compile(await Generate([.. command.Options], command.PositionalArguments,
            command.RequiredAlternativeGroups)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;

        (string? Type, string? Name, bool? All, string? Selector, string? FieldSelector, string[]? Filename, string? Kustomize, bool Valid)[] cases =
        [
            ("pods", null, null, null, null, null, null, false),
            ("pods", null, false, null, null, null, null, false),
            ("pods/", null, null, null, null, null, null, false),
            ("/web", null, null, null, null, null, null, false),
            ("pods", " ", null, " ", " ", null, null, false),
            (null, null, true, null, null, null, null, false),
            (null, null, null, "app=web", null, null, null, false),
            ("pods", "web", null, null, null, null, null, true),
            ("pods/web", null, null, null, null, null, null, true),
            ("pods", null, true, null, null, null, null, true),
            ("pods", null, null, "app=web", null, null, null, true),
            ("pods", null, null, null, "metadata.name=web", null, null, true),
            (null, null, null, null, null, ["manifest.yaml"], null, true),
            (null, null, null, null, null, null, "overlay", true),
            ("pods", null, null, null, null, ["manifest.yaml"], null, true),
        ];
        foreach (var input in cases)
        {
            var instance = Activator.CreateInstance(options, [KubectlResourceUpdates])!;
            options.GetProperty("Type")!.SetValue(instance, input.Type);
            options.GetProperty("Name")!.SetValue(instance, input.Name);
            options.GetProperty("All")!.SetValue(instance, input.All);
            options.GetProperty("Selector")!.SetValue(instance, input.Selector);
            options.GetProperty("FieldSelector")!.SetValue(instance, input.FieldSelector);
            options.GetProperty("Filename")!.SetValue(instance, input.Filename);
            options.GetProperty("Kustomize")!.SetValue(instance, input.Kustomize);
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(input.Valid).Because(input.ToString());
        }
    }

    private sealed class ResourceFixtureExecutor(string commandName, string help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => $"Available Commands:\n  {commandName}  Update resources\n",
                    "version --client" => "Client Version: v1.37.1",
                    _ when arguments == commandName + " --help" => help,
                    _ => throw new InvalidOperationException($"Unexpected command: {arguments}"),
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
