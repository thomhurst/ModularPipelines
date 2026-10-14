using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Tests.Scrapers;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class PackageManagerGlobalRenderingTests
{
    [Test]
    [Arguments("yarn")]
    [Arguments("npm")]
    [Arguments("pnpm")]
    public async Task Generated_Global_Properties_Render_Their_Documented_Value_Shapes(string name)
    {
        var tool = await NodePackageManagerGlobalOptionsTests.Scrape(name);
        // Compile the actual base and an operand-free command, keeping this regression
        // focused on inherited options rather than install's changing required operands.
        var command = tool.Commands.Single() with { Options = [], PositionalArguments = [], Enums = [] };
        tool = tool with { Commands = [command] };
        var files = (await new GlobalOptionsBaseGenerator().GenerateAsync(tool))
            .Concat(await new OptionsClassGenerator().GenerateAsync(tool))
            .Concat(await new EnumGenerator().GenerateAsync(tool));
        var assembly = Compile(files.Select(file => file.Content));
        var type = assembly.GetType($"{tool.TargetNamespace}.Options.{command.ClassName}")!;
        var instance = Activator.CreateInstance(type)!;
        (string Property, object Value)[] values = name switch
        {
            "yarn" => [("Cwd", "project path")],
            "npm" => [("Prefix", "project path"), ("Timing", false), ("LogsMax", 0), ("FetchRetryFactor", 1.5), ("Ca", new[] { "cert-one", "cert-two" })],
            _ => [("Dir", "project path"), ("Color", (CliOptionValue) "always"), ("WorkspacePackages", new[] { "apps/*", "libs/*" })],
        };
        var parts = new List<PropertyCommandLinePart>();
        foreach (var (propertyName, value) in values)
        {
            var property = type.GetProperty(propertyName)!;
            property.SetValue(instance, value);
            parts.Add(new OptionPart(property.Name, property.GetValue, property.GetCustomAttribute<CliOptionAttribute>()!));
            await Assert.That(property.DeclaringType).IsEqualTo(type.BaseType);
        }

        string[] expected = name switch
        {
            "yarn" => ["--cwd", "project path"],
            "npm" => ["--prefix=project path", "--timing=false", "--logs-max=0", "--fetch-retry-factor=1.5", "--ca=cert-one", "--ca=cert-two"],
            _ => ["--dir", "project path", "--color=always", "--workspace-packages", "apps/*", "--workspace-packages", "libs/*"],
        };
        await Assert.That(new CommandArgumentBuilder().BuildArguments(parts, instance))
            .IsEquivalentTo(expected, TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(type.BaseType!.CustomAttributes.Any(attribute => attribute.AttributeType.Name == "CliGlobalOptionsAttribute"))
            .IsEqualTo(name == "yarn");
    }

    private static Assembly Compile(IEnumerable<string> sources)
    {
        const string support = """
            global using System.Collections.Generic;
            namespace ModularPipelines.Attributes
            {
                public sealed class CliToolAttribute(string name) : System.Attribute;
                public sealed class CliSubCommandAttribute(params string[] parts) : System.Attribute;
                public sealed class CliGlobalOptionsAttribute : System.Attribute;
            }
            namespace ModularPipelines.Secrets
            {
                public sealed class SecretValueAttribute : System.Attribute;
            }
            namespace ModularPipelines.Options
            {
                public record CommandLineToolOptions;
            }
            """;
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Append(typeof(CliOptionAttribute).Assembly.Location).Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(Guid.NewGuid().ToString("N"),
            sources.Prepend(support).Select(source => CSharpSyntaxTree.ParseText(source,
                CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview))), references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        if (!result.Success)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
        }

        return System.Reflection.Assembly.Load(stream.ToArray());
    }
}
