using System.Reflection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Tests.Generators;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public partial class DotNetCliScraperTests
{
    private static readonly string[] RepeatablePackageSources = ["https://private.example/feed", "https://api.nuget.org/v3/index.json"];

    [Test]
    [Arguments("restore", "source", true)]
    [Arguments("build", "source", true)]
    [Arguments("restore", "runtime", true)]
    [Arguments("build", "runtime", false)]
    [Arguments("tool install", "add-source", true)]
    [Arguments("tool install", "source", true)]
    [Arguments("tool update", "add-source", true)]
    [Arguments("tool update", "source", true)]
    [Arguments("tool restore", "add-source", true)]
    [Arguments("tool execute", "add-source", true)]
    [Arguments("tool execute", "source", true)]
    [Arguments("workload install", "source", true)]
    [Arguments("workload update", "source", true)]
    [Arguments("workload restore", "source", true)]
    [Arguments("workload repair", "source", true)]
    [Arguments("new install", "nuget-source", true)]
    [Arguments("new update", "nuget-source", true)]
    [Arguments("new details", "nuget-source", true)]
    [Arguments("new install", "add-source", true)]
    [Arguments("new update", "add-source", true)]
    [Arguments("new details", "add-source", true)]
    [Arguments("nuget push", "source", false)]
    [Arguments("nuget add source", "source", false)]
    [Arguments("package add", "source", false)]
    [Arguments("restore", "packages", false)]
    public async Task Sdk_Collection_Options_Do_Not_Depend_On_Help_Wording(
        string subcommand, string optionName, bool repeatable)
    {
        var help = $"Options:\n  --{optionName} <VALUE>  Select a value.\n";
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", .. subcommand.Split(' ')], help))!;
        var enhanced = await OptionTypeEnhancer.CreateDefault(new DotNet11HelpExecutor(help), NullLoggerFactory.Instance)
            .EnhanceAsync(DotNet11Tool(command));
        var option = enhanced.Commands.Single().Options.Single(option => option.SwitchName == $"--{optionName}");

        await Assert.That(option.AcceptsMultipleValues).IsEqualTo(repeatable);
        await Assert.That(option.CSharpType).IsEqualTo(repeatable ? "IEnumerable<string>?" : "string?");
        await Assert.That(option.IsFlag).IsFalse();
        var generated = (await new OptionsClassGenerator().GenerateAsync(enhanced)).Single().Content;
        await Assert.That(generated).Contains($"public {option.CSharpType} {option.PropertyName}");
    }

    [Test]
    [Arguments("restore", "source")]
    [Arguments("tool install", "add-source")]
    [Arguments("new install", "nuget-source")]
    public async Task Generated_Source_Collections_Render_Each_Source_With_Its_Own_Switch(string subcommand, string optionName)
    {
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", .. subcommand.Split(' ')],
            $"Options:\n  --{optionName} <SOURCE>  The NuGet package source to use.\n"))!;
        var option = command.Options.Single(option => option.SwitchName == $"--{optionName}");
        command = command with { Options = [option], PositionalArguments = [] };
        var tool = DotNet11Tool(command);
        var files = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool))
            .Concat(await new OptionsClassGenerator().GenerateAsync(tool));
        var type = PackageManagerGlobalRenderingTests.Compile(files.Select(file => file.Content))
            .GetType($"{tool.TargetNamespace}.Options.{command.ClassName}")!;
        var instance = Activator.CreateInstance(type)!;
        var property = type.GetProperty(option.PropertyName)!;
        property.SetValue(instance, RepeatablePackageSources);
        PropertyCommandLinePart[] parts =
        [
            new OptionPart(property.Name, property.GetValue, property.GetCustomAttribute<CliOptionAttribute>()!),
        ];

        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo([$"--{optionName}", RepeatablePackageSources[0], $"--{optionName}", RepeatablePackageSources[1]],
                TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Mentioning_Multiple_Projects_Does_Not_Make_A_Flag_Repeatable()
    {
        var command = (await new TestDotNetCliScraper().Parse(["dotnet", "restore"],
            "Options:\n  --disable-parallel  Prevent restoring multiple projects in parallel.\n"))!;
        var option = command.Options.Single(option => option.SwitchName == "--disable-parallel");
        await Assert.That(option.IsFlag).IsTrue();
        await Assert.That(option.AcceptsMultipleValues).IsFalse();
    }
}
