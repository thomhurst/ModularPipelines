using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    private static readonly string[] KubectlResourceUpdates = ["owner=team"];
    private static readonly string[] ManualFilenameArguments = ["--filename", "manifest.yaml"];
    private static readonly string[] TypedFilename = ["manifest.yaml"];

    [Test]
    [Arguments("annotate")]
    [Arguments("label")]
    [Arguments("patch")]
    [Arguments("scale")]
    public async Task Kubectl_Manual_Filename_Does_Not_Satisfy_Typed_Resource_Validation(string commandName)
    {
        var command = await ScrapeResourceCommand(commandName, $$"""
            Usage:
              kubectl {{commandName}} (-f FILENAME | TYPE NAME) [options]

            Options:
              -f, --filename stringArray   Files identifying resources.
            """);
        var generated = await Generate([.. command.Options], command.PositionalArguments,
            command.RequiredAlternativeGroups);
        var options = Compile(generated, """
            namespace ModularPipelines.Tool.Options
            {
                public partial record ToolOptions
                {
                    public System.Collections.Generic.IEnumerable<string>? Arguments { get; init; }
                }
            }
            """).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Arguments")!.SetValue(instance, ManualFilenameArguments);
        var validation = (IValidatableObject) instance;

        await Assert.That(validation.Validate(new(instance))).IsNotEmpty();

        options.GetProperty("Filename")!.SetValue(instance, TypedFilename);
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
    }

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
        var command = await ScrapeResourceCommand(commandName, help);
        var options = Compile(await Generate([.. command.Options], command.PositionalArguments,
            command.RequiredAlternativeGroups)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        await Assert.That(options.GetProperty("Type")).IsNotNull();
        await Assert.That(options.GetProperty("Name")).IsNotNull();

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

        var target = Activator.CreateInstance(options, [KubectlResourceUpdates])!;
        options.GetProperty("Type")!.SetValue(target, "pods");
        options.GetProperty("Name")!.SetValue(target, "web");
        await Assert.That(RenderResourceOperands(target)).IsEquivalentTo(["pods", "web", "owner=team"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("patch")]
    [Arguments("scale")]
    public async Task Kubectl_Resource_Alternatives_Preserve_Patch_And_Scale_Contracts(string commandName)
    {
        var patchTypeOption = commandName == "patch" ? "--type string           Patch strategy." : string.Empty;
        var help = $$"""
            Usage:
              kubectl {{commandName}} (-f FILENAME | TYPE NAME) [options]

            Options:
              -f, --filename stringArray   Files identifying resources.
              -k, --kustomize string       Kustomization directory.
                  {{patchTypeOption}}
            """;
        var command = await ScrapeResourceCommand(commandName, help);
        var options = Compile(await Generate([.. command.Options], command.PositionalArguments,
            command.RequiredAlternativeGroups)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var typeProperty = commandName == "patch" ? "TypeArgument" : "Type";
        await Assert.That(options.GetProperty(typeProperty)).IsNotNull();

        (string? Type, string? Name, string[]? Filename, string? Kustomize, bool Valid)[] cases =
        [
            (null, null, null, null, false),
            (null, "web", null, null, false),
            ("pods", null, null, null, false),
            ("pods/", null, null, null, false),
            ("/web", null, null, null, false),
            ("pods", " ", null, null, false),
            ("pods", "web", null, null, true),
            ("pods/web", null, null, null, true),
            (null, null, ["manifest.yaml"], null, true),
            (null, null, null, "overlay", true),
        ];
        foreach (var input in cases)
        {
            var instance = Activator.CreateInstance(options)!;
            options.GetProperty(typeProperty)!.SetValue(instance, input.Type);
            options.GetProperty("Name")!.SetValue(instance, input.Name);
            options.GetProperty("Filename")!.SetValue(instance, input.Filename);
            options.GetProperty("Kustomize")!.SetValue(instance, input.Kustomize);
            if (commandName == "patch")
            {
                options.GetProperty("Type")!.SetValue(instance, "merge");
            }
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(input.Valid).Because(input.ToString());
            if (input.Type is not null)
            {
                string[] expected = input.Name is null ? [input.Type] : [input.Type, input.Name];
                await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo(expected,
                    TUnit.Assertions.Enums.CollectionOrdering.Matching);
            }
        }

        var tool = new CliToolDefinition
        {
            ToolName = "kubectl",
            NamespacePrefix = "Kubernetes",
            TargetNamespace = "ModularPipelines.Kubernetes",
            OutputDirectory = "output",
            GenerateCommandFacade = true,
            Commands = [command],
        };
        foreach (var generator in new ICodeGenerator[] { new ServiceInterfaceGenerator(), new ServiceImplementationGenerator() })
        {
            var service = (await generator.GenerateAsync(tool)).Single().Content;
            await Assert.That(service).Contains($"{command.ClassName} options,");
            await Assert.That(service).DoesNotContain($"{command.ClassName}? options");
        }
    }

    [Test]
    public async Task Kubectl_Scale_Resource_Alternatives_Accept_Selection_Without_A_Name()
    {
        var command = await ScrapeResourceCommand("scale", """
            Usage:
              kubectl scale (-f FILENAME | TYPE NAME) --replicas=COUNT [options]

            Options:
              -f, --filename stringArray   Files identifying resources.
                  --all=false: Select all resources of the specified types.
              -l, --selector string       Label selector.
                  --replicas int         Number of replicas.
            """);
        var options = Compile(await Generate([.. command.Options], command.PositionalArguments,
            command.RequiredAlternativeGroups)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        (string? Type, bool? All, string? Selector, bool Valid)[] cases =
        [
            ("deployment", true, null, true),
            ("deployment", null, "app=web", true),
            ("deployment", false, "app=web", true),
            ("deployment", null, null, false),
            ("deployment", false, null, false),
            ("deployment", null, " ", false),
            ("deployment", false, " ", false),
            (null, true, null, false),
            (null, null, "app=web", false),
            (" ", true, null, false),
        ];
        foreach (var input in cases)
        {
            var instance = Activator.CreateInstance(options)!;
            options.GetProperty("Type")!.SetValue(instance, input.Type);
            options.GetProperty("All")!.SetValue(instance, input.All);
            options.GetProperty("Selector")!.SetValue(instance, input.Selector);
            options.GetProperty("Replicas")!.SetValue(instance, 3);
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(input.Valid).Because(input.ToString());
            if (input.Valid)
            {
                await Assert.That(RenderResourceOperands(instance)).IsEquivalentTo([input.Type!],
                    TUnit.Assertions.Enums.CollectionOrdering.Matching);
            }
        }
    }

    private static readonly string[] valueArray = ["manifest.yaml"];

    [Test]
    [Arguments("patch")]
    [Arguments("scale")]
    [Arguments("label")]
    public async Task Kubectl_Direct_Combined_Target_Requires_Both_Type_And_Name(string commandName)
    {
        var command = await ScrapeResourceCommand(commandName, $$"""
            Usage:
              kubectl {{commandName}} (-f FILENAME | TYPE/NAME) [options]

            Options:
              -f, --filename stringArray   Files identifying resources.
            """);
        var options = Compile(await Generate([.. command.Options], command.PositionalArguments,
            command.RequiredAlternativeGroups)).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        string[] targets = ["pods/web", "pods/", "/web", "pods", "pods/web/extra", " "];
        foreach (var target in targets)
        {
            var instance = Activator.CreateInstance(options)!;
            options.GetProperty("TypeOrName")!.SetValue(instance, target);
            await Assert.That(!((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(target == "pods/web").Because(target);
        }
        var file = Activator.CreateInstance(options)!;
        options.GetProperty("Filename")!.SetValue(file, valueArray);
        await Assert.That(((IValidatableObject) file).Validate(new(file))).IsEmpty();
    }

    private static IReadOnlyList<string> RenderResourceOperands(object instance)
    {
        var parts = instance.GetType().GetProperties().SelectMany(property => property.CustomAttributes
            .Where(attribute => attribute.AttributeType.Name == nameof(CliArgumentAttribute))
            .Select(attribute => new ArgumentPart(property.Name, property.GetValue,
                new CliArgumentAttribute((int) attribute.ConstructorArguments[0].Value!)
                {
                    Phase = (CommandLinePhase) (int) attribute.NamedArguments.Single(argument => argument.MemberName == "Phase").TypedValue.Value!,
                })));
        return new CommandArgumentBuilder().BuildArguments([.. parts], instance);
    }

    private static async Task<CliCommandDefinition> ScrapeResourceCommand(string commandName, string help)
    {
        var scraper = new KubectlCliScraper(new ResourceFixtureExecutor(commandName, help),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<KubectlCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var definition in scraper.ScrapeAsync())
        {
            commands.Add(definition);
        }
        return commands.Single();
    }

    private sealed class ResourceFixtureExecutor(string commandName, string help) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            string? workingDirectory = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments switch
                {
                    "--help" => $"Available Commands:\n  {commandName}  Update resources\n",
                    "options" => """
                        The following options can be passed to any command:
                          --namespace string    Namespace scope.
                          --context string      Context to use.
                          --kubeconfig string   Path to kubeconfig.
                        """,
                    "version --client" => "Client Version: v1.37.1",
                    _ when arguments == commandName + " --help" => help,
                    _ => throw new InvalidOperationException($"Unexpected command: {arguments}"),
                },
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
