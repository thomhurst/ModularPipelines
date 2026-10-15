using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    private static readonly string[] KubectlSetImageUpdates = ["nginx=nginx:1.9.1"];

    [Test]
    [Arguments("resources")]
    [Arguments("image")]
    public async Task Kubectl_Set_Commands_Validate_Complete_Resource_Selections(string commandName)
    {
        var command = await ScrapeSetCommand(commandName);
        var expected = commandName == "image"
            ? new[] { "Type", "Name", "ContainerNameContainerImage" }
            : ["Type", "Name"];
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(expected, TUnit.Assertions.Enums.CollectionOrdering.Matching);
        var options = await CompileSetOptions(command);
        (string? Type, string? Name, bool? All, string? Selector, string[]? Filename, string? Kustomize, bool Valid)[] cases =
        [
            ("deployments,rc", null, true, null, null, null, true),
            ("deployment", null, null, "app=web", null, null, true),
            ("deployment", "nginx", null, null, null, null, true),
            ("deployment/nginx", null, null, null, null, null, true),
            (null, null, null, null, ["manifest.yaml"], null, true),
            (null, null, null, null, null, "overlay", true),
            ("deployment", null, false, null, null, null, false),
            ("deployment", null, null, " ", null, null, false),
            (null, null, true, null, null, null, false),
            ("deployment/", null, null, null, null, null, false),
            ("/nginx", null, null, null, null, null, false),
        ];
        foreach (var input in cases)
        {
            var instance = commandName == "image"
                ? Activator.CreateInstance(options, [KubectlSetImageUpdates])!
                : Activator.CreateInstance(options)!;
            options.GetProperty("Type")!.SetValue(instance, input.Type);
            options.GetProperty("Name")!.SetValue(instance, input.Name);
            options.GetProperty("All")!.SetValue(instance, input.All);
            options.GetProperty("Selector")!.SetValue(instance, input.Selector);
            options.GetProperty("Filename")!.SetValue(instance, input.Filename);
            options.GetProperty("Kustomize")!.SetValue(instance, input.Kustomize);
            if (commandName == "resources")
            {
                options.GetProperty("Limits")!.SetValue(instance, "cpu=200m");
            }
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(input.Valid).Because(input.ToString());
            if (input.Valid)
            {
                string[] operands = [.. new[] { input.Type, input.Name }.OfType<string>(),
                    .. commandName == "image" ? new[] { "nginx=nginx:1.9.1" } : []];
                await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo(operands,
                    TUnit.Assertions.Enums.CollectionOrdering.Matching);
            }
        }
    }

    [Test]
    public async Task Kubectl_Set_Resources_Requires_Limits_Or_Requests()
    {
        var options = await CompileSetOptions(await ScrapeSetCommand("resources"));
        (string? Limits, string? Requests, bool Valid)[] cases =
        [
            (null, null, false),
            (" ", "", false),
            ("cpu=200m", null, true),
            (null, "cpu=100m", true),
            ("cpu=200m", "cpu=100m", true),
        ];
        foreach (var input in cases)
        {
            var instance = Activator.CreateInstance(options)!;
            options.GetProperty("Type")!.SetValue(instance, "deployment");
            options.GetProperty("Name")!.SetValue(instance, "nginx");
            options.GetProperty("Limits")!.SetValue(instance, input.Limits);
            options.GetProperty("Requests")!.SetValue(instance, input.Requests);
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(input.Valid).Because(input.ToString());
            await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo(["deployment", "nginx"],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
        }
    }

    [Test]
    public async Task Kubectl_Set_Env_Accepts_Alternate_Targets_And_Operations()
    {
        var command = await ScrapeSetCommand("env");
        var options = await CompileSetOptions(command);
        await Assert.That(options.GetConstructors().Single().GetParameters()).IsEmpty();
        (string? Type, string? Name, bool? All, string? Selector, string[]? Filename, string? Kustomize,
            string[]? KeyVal, bool? List, string? From, string[]? Env, bool Valid)[] cases =
        [
            ("deployment/sample-build", null, null, null, null, null, null, true, null, null, true),
            ("deployment", "myapp", null, null, null, null, null, null, "secret/mysecret", null, true),
            ("deployment/myapp", null, null, null, null, null, null, null, "configmap/myconfigmap", null, true),
            ("rc", null, true, null, null, null, ["ENV=prod"], null, null, null, true),
            ("pods", null, null, "app=web", null, null, null, true, null, null, true),
            (null, null, null, null, ["deploy.json"], null, ["ENV-"], null, null, null, true),
            (null, null, null, null, null, "overlay", null, null, null, ["ENV=prod"], false),
            ("deployment/registry", null, null, null, null, null, null, null, null, ["-"], true),
            ("deployment/registry", null, null, null, null, null, null, false, null, null, false),
            ("deployment/registry", null, null, null, null, null, [], null, " ", [], false),
            (null, null, true, null, null, null, ["ENV=prod"], null, null, null, false),
            ("deployment", null, false, null, null, null, null, true, null, null, false),
            ("deployment", null, null, " ", null, null, ["ENV=prod"], null, null, null, false),
            ("deployment/", null, null, null, null, null, null, true, null, null, false),
            ("/registry", null, null, null, null, null, null, true, null, null, false),
            (null, null, null, null, [], null, null, true, null, null, false),
        ];
        foreach (var input in cases)
        {
            var instance = Activator.CreateInstance(options)!;
            foreach (var (name, value) in new (string, object?)[]
                     {
                         ("Type", input.Type), ("Name", input.Name), ("All", input.All), ("Selector", input.Selector),
                         ("Filename", input.Filename), ("Kustomize", input.Kustomize), ("KeyVal", input.KeyVal),
                         ("List", input.List), ("From", input.From), ("Env", input.Env),
                     })
            {
                options.GetProperty(name)!.SetValue(instance, value);
            }
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(input.Valid).Because(input.ToString());
            if (input.Valid)
            {
                string[] operands = [.. new[] { input.Type, input.Name }.OfType<string>(), .. input.KeyVal ?? []];
                await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo(operands,
                    TUnit.Assertions.Enums.CollectionOrdering.Matching);
            }
        }
    }

    private static async Task<Type> CompileSetOptions(CliCommandDefinition command) =>
        Compile(await Generate([.. command.Options.Select(option => option.EnumDefinition is null ? option
                : option with { CSharpType = "string?", EnumDefinition = null })],
            command.PositionalArguments, command.RequiredAlternativeGroups))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;

    private static async Task<CliCommandDefinition> ScrapeSetCommand(string commandName)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Kubectl", "1.37.1", $"set-{commandName}.txt"));
        var scraper = new KubectlCliScraper(new SetFixtureExecutor(commandName, help),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<KubectlCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }
        return commands.Single();
    }

    private sealed class SetFixtureExecutor(string commandName, string help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            string? workingDirectory = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => "Available Commands:\n  set  Update resources\n",
                    "set --help" => $"Available Commands:\n  {commandName}  Update resources\n",
                    "options" => """
                        The following options can be passed to any command:
                          --namespace string    Namespace scope.
                          --context string      Context to use.
                          --kubeconfig string   Path to kubeconfig.
                        """,
                    "version --client" => "Client Version: v1.37.1",
                    _ when arguments == $"set {commandName} --help" => help,
                    _ => throw new InvalidOperationException($"Unexpected command: {arguments}"),
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
