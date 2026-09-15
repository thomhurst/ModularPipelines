using System.ComponentModel.DataAnnotations;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class CliScraperTraversalTests
{
    [Test]
    [Arguments("--verbose <TARGET>", false)]
    [Arguments("--verbose <TARGET>", true)]
    [Arguments("[--verbose] <TARGET>", false)]
    public async Task Cargo_Usage_Alternatives_Retain_Required_Flag_And_Operand(string positionalForm, bool fileFirst)
    {
        const string fileForm = "cargo run [OPTIONS] --file <FILE>";
        var targetForm = $"cargo run [OPTIONS] {positionalForm}";
        var executor = new StubExecutor(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--help"] = """
                Usage: cargo [OPTIONS] <COMMAND>

                Commands:
                  run  Execute a package

                Options:
                  -h, --help  Print help
                """,
            ["run --help"] = $"""
                Execute a package

                Usage: {(fileFirst ? fileForm : targetForm)}
                       {(fileFirst ? targetForm : fileForm)}

                Options:
                      --file <FILE>  Read a file
                  -v, --verbose      Print detailed output
                """,
        });
        var command = (await ScrapeAsync(new TestCargoCliScraper(executor))).Single();
        await Assert.That(command.Options.Single(option => option.PropertyName == "Verbose").IsFlag).IsTrue();
        await Assert.That(command.Options.Single(option => option.PropertyName == "File").IsFlag).IsFalse();
        await Assert.That(command.PositionalArguments.Single().PropertyName).IsEqualTo("Target");
        await Assert.That(command.PositionalArguments.Single().IsRequired).IsFalse();

        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "cargo",
            NamespacePrefix = "Cargo",
            TargetNamespace = "ModularPipelines.Rust",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        await VerifyCargoUsageValidation(generated, command.ClassName, async type =>
        {
            var flagRequired = !positionalForm.StartsWith('[');
            (string? File, bool? Verbose, string? Target, bool Valid)[] cases =
            [
                (null, null, null, false),
                ("file", null, null, true),
                ("file", true, null, true),
                ("file", null, "target", true),
                ("file", true, "target", true),
                (null, true, "target", true),
                (null, true, null, false),
                (null, null, "target", !flagRequired),
                (null, false, "target", !flagRequired),
            ];
            foreach (var testCase in cases)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("File")!.SetValue(instance, testCase.File);
                type.GetProperty("Verbose")!.SetValue(instance, testCase.Verbose);
                type.GetProperty("Target")!.SetValue(instance, testCase.Target);
                var valid = !((IValidatableObject) instance).Validate(new(instance)).Any();
                await Assert.That(valid).IsEqualTo(testCase.Valid);
            }
        });
    }

    private static async Task VerifyCargoUsageValidation(string generated, string className, Func<Type, Task> verify)
    {
        const string support = """
            global using System;
            global using System.Collections.Generic;
            namespace ModularPipelines.Rust.Options { public record CargoOptions; }
            namespace ModularPipelines.Attributes
            {
                public sealed class CliOptionAttribute(string name) : Attribute;
                public sealed class CliFlagAttribute(string name) : Attribute
                {
                    public string? ShortForm { get; set; }
                }
                public sealed class CliSubCommandAttribute(params string[] parts) : Attribute;
                public sealed class CliArgumentAttribute(int position) : Attribute
                {
                    public CommandLinePhase Phase { get; set; }
                }
            }
            """;
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator).Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create("cargo-usage-validation",
            [CSharpSyntaxTree.ParseText(support), CSharpSyntaxTree.ParseText(generated)], references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        await Assert.That(result.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(diagnostic => diagnostic.ToString())).IsEmpty();
        stream.Position = 0;
        var loadContext = new AssemblyLoadContext("cargo-usage-validation", isCollectible: true);
        try
        {
            await verify(loadContext.LoadFromStream(stream).GetType($"ModularPipelines.Rust.Options.{className}")!);
        }
        finally
        {
            loadContext.Unload();
        }
    }
}
