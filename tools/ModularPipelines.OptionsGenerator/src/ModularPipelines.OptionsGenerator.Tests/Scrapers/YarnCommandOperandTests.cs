using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.Tests.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class YarnCommandOperandTests
{
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
