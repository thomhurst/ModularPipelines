using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.Tests.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class YarnCommandOperandTests
{
    [Test]
    [Arguments("workspaces foreach", "All")]
    [Arguments("workspaces foreach", "Recursive")]
    [Arguments("workspaces foreach", "Worktree")]
    [Arguments("workspaces foreach", "Since")]
    [Arguments("dlx", "Quiet")]
    [Arguments("workspace", null)]
    public async Task Proxy_Commands_Render_Options_Before_Child_Arguments(string commandName, string? optionName)
    {
        var fixtureDirectory = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Yarn", "4.18.1");
        var rootHelp = await File.ReadAllTextAsync(Path.Combine(fixtureDirectory, "yarn-help.txt"));
        var discoveryLine = rootHelp.Split('\n').Single(line =>
            line.TrimStart().StartsWith($"yarn {commandName} ", StringComparison.Ordinal));
        var childHelp = await File.ReadAllTextAsync(Path.Combine(fixtureDirectory,
            $"yarn-{commandName.Replace(' ', '-')}-help.txt"));
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FixtureYarnScraper(new FixtureExecutor(
            $"General commands\n\n{discoveryLine}\n", childHelp), cache);
        var commands = new List<CliCommandDefinition>();
        await foreach (var discovered in scraper.ScrapeAsync())
        {
            commands.Add(discovered);
        }

        var command = commands.Single();
        var expectedPhase = commandName == "workspace" ? CommandLinePhase.EarlyOperand : CommandLinePhase.Passthrough;
        await Assert.That(command.PositionalArguments.All(argument => argument.Phase == expectedPhase)).IsTrue();
        await Assert.That(command.PositionalArguments.Last().IsRequired).IsTrue();
        await Assert.That(command.PositionalArguments.Last().IsVariadic).IsTrue();

        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var files = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool))
            .Concat(await new OptionsClassGenerator().GenerateAsync(tool));
        var assembly = PackageManagerGlobalRenderingTests.Compile(files.Select(file => file.Content));
        var type = assembly.GetType($"{tool.TargetNamespace}.Options.{command.ClassName}")!;
        string[] childArguments = ["node", "script with spaces.js", "--all", "--", "--recursive"];
        object[] constructorArguments = commandName == "workspace" ? ["components", childArguments] : [childArguments];
        var instance = Activator.CreateInstance(type, constructorArguments)!;
        var parts = new List<PropertyCommandLinePart>();
        foreach (var argument in command.PositionalArguments)
        {
            var property = type.GetProperty(argument.PropertyName)!;
            var attribute = property.GetCustomAttribute<CliArgumentAttribute>()!;
            await Assert.That(attribute.Phase).IsEqualTo(expectedPhase);
            parts.Add(new ArgumentPart(property.Name, property.GetValue, attribute) { HasExplicitPosition = true });
        }

        string[] expectedPrefix = commandName == "workspace" ? ["components"] : [];
        if (optionName is not null)
        {
            var property = type.GetProperty(optionName)!;
            if (optionName == "Since")
            {
                property.SetValue(instance, (CliOptionValue) "main");
                parts.Add(new OptionPart(property.Name, property.GetValue, property.GetCustomAttribute<CliOptionAttribute>()!));
                expectedPrefix = ["--since=main"];
            }
            else
            {
                property.SetValue(instance, true);
                parts.Add(new FlagPart(property.Name, property.GetValue, property.GetCustomAttribute<CliFlagAttribute>()!));
                expectedPrefix = [$"--{optionName.ToLowerInvariant()}"];
            }
        }

        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo([.. expectedPrefix, .. childArguments], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("npm tag add", "Package,Tag", true)]
    [Arguments("npm tag remove", "Package,Tag", true)]
    [Arguments("plugin remove", "Name", true)]
    [Arguments("set resolution", "Descriptor,Resolution", true)]
    [Arguments("explain peer-requirements", "Hash", false)]
    public async Task Discovered_Nested_Commands_Preserve_Their_Operand_Contracts(
        string commandName, string operandNames, bool required)
    {
        var fixtureDirectory = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Yarn", "4.18.1");
        var rootHelp = await File.ReadAllTextAsync(Path.Combine(fixtureDirectory, "yarn-help.txt"));
        // Retain the captured discovery line, including its packed multiword command path.
        var discoveryLine = rootHelp.Split('\n').Single(line =>
            line.TrimStart().StartsWith($"yarn {commandName} ", StringComparison.Ordinal));
        var childHelp = await File.ReadAllTextAsync(Path.Combine(fixtureDirectory,
            $"yarn-{commandName.Replace(' ', '-')}-help.txt"));
        using var cache = new HelpTextCache(NullLogger<HelpTextCache>.Instance);
        var scraper = new FixtureYarnScraper(new FixtureExecutor(
            $"General commands\n\n{discoveryLine}\n", childHelp), cache);
        var commands = new List<CliCommandDefinition>();
        await foreach (var discovered in scraper.ScrapeAsync())
        {
            commands.Add(discovered);
        }

        var command = commands.Single();
        var names = operandNames.Split(',');
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(names, TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(command.PositionalArguments.Select(argument => argument.IsRequired))
            .IsEquivalentTo(names.Select(_ => required));
        await Assert.That(command.PositionalArguments.Select(argument => argument.PositionIndex))
            .IsEquivalentTo(Enumerable.Range(0, names.Length), TUnit.Assertions.Enums.CollectionOrdering.Matching);

        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var files = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool))
            .Concat(await new OptionsClassGenerator().GenerateAsync(tool));
        var assembly = PackageManagerGlobalRenderingTests.Compile(files.Select(file => file.Content));
        var type = assembly.GetType($"{tool.TargetNamespace}.Options.{command.ClassName}")!;
        await Assert.That(type.GetConstructors().Single().GetParameters().Select(parameter => parameter.Name!))
            .IsEquivalentTo(required ? names : [], TUnit.Assertions.Enums.CollectionOrdering.Matching);
        var values = names.Select(name => name.ToLowerInvariant()).ToArray();
        var instance = Activator.CreateInstance(type, required ? values.Cast<object>().ToArray() : [])!;
        var parts = new List<PropertyCommandLinePart>();
        for (var index = 0; index < names.Length; index++)
        {
            var property = type.GetProperty(names[index])!;
            var attribute = property.GetCustomAttribute<CliArgumentAttribute>()!;
            await Assert.That(attribute.Required).IsEqualTo(required);
            property.SetValue(instance, values[index]);
            parts.Add(new ArgumentPart(property.Name, property.GetValue, attribute));
        }

        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo(values, TUnit.Assertions.Enums.CollectionOrdering.Matching);
        var signature = new StringBuilder();
        GeneratorUtils.GenerateServiceMethodSignature(signature, "Execute", command);
        await Assert.That(signature.ToString()).Contains(required
            ? $"({command.ClassName} options,"
            : $"({command.ClassName}? options = null,");
    }

    private sealed class FixtureYarnScraper(ICliCommandExecutor executor, IHelpTextCache cache)
        : YarnCliScraper(executor, cache, NullLogger<YarnCliScraper>.Instance)
    {
        public override Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);
    }

    private sealed class FixtureExecutor(string root, string child) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            string? workingDirectory = null, CancellationToken cancellationToken = default) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "--help" ? root : arguments == "--version" ? "4.18.1" : child,
                StandardError = "",
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
