using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    ModularPipelines.Analyzers.CliPropertyAttributeAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CliPropertyAttributeAnalyzerTests
{
    [TestMethod]
    public void Diagnostic_Ids_Are_Unique()
    {
        var duplicates = typeof(CliPropertyAttributeAnalyzer).Assembly
            .GetTypes()
            .SelectMany(static type => type.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(static field => field.IsLiteral
                                   && field.FieldType == typeof(string)
                                   && field.Name.EndsWith("DiagnosticId", StringComparison.Ordinal))
            .Select(static field => (string)field.GetRawConstantValue()!)
            .GroupBy(static id => id, StringComparer.Ordinal)
            .Where(static group => group.Count() > 1)
            .Select(static group => group.Key)
            .ToArray();

        Assert.AreEqual(0, duplicates.Length, $"Duplicate diagnostic IDs: {string.Join(", ", duplicates)}");
    }

    [TestMethod]
    public async Task Reports_Invalid_Flag_Type()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("tool")]
            public record Options : CommandLineToolOptions
            {
                [{|#0:CliFlag("--name")|}]
                public string? Name { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliPropertyAttributeAnalyzer.InvalidFlagTypeDiagnosticId)
            .WithLocation(0)
            .WithArguments("Name", "string?");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Accepts_Boolean_And_Integer_Flags()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("tool")]
            public record Options : CommandLineToolOptions
            {
                [CliFlag("--force")]
                public bool? Force { get; init; }

                [CliFlag("--verbose")]
                public int? Verbosity { get; init; }

                [CliFlag("--force-required")]
                public bool ForceRequired { get; init; }

                [CliFlag("--verbose-required")]
                public int VerbosityRequired { get; init; }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Negated_Flag_On_NonNullable_Or_Counted_Type()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("tool")]
            public record Options : CommandLineToolOptions
            {
                [{|#0:CliFlag("--feature", NegatedName = "--no-feature")|}]
                public bool Feature { get; init; }

                [{|#1:CliFlag("--verbose", NegatedName = "--quiet")|}]
                public int? Verbosity { get; init; }
            }
            """;

        var nonNullableBoolean = VerifyCS.Diagnostic(CliPropertyAttributeAnalyzer.NegatedFlagTypeDiagnosticId)
            .WithLocation(0)
            .WithArguments("Feature", "bool");
        var countedFlag = VerifyCS.Diagnostic(CliPropertyAttributeAnalyzer.NegatedFlagTypeDiagnosticId)
            .WithLocation(1)
            .WithArguments("Verbosity", "int?");

        await VerifyCS.VerifyAnalyzerAsync(source, nonNullableBoolean, countedFlag);
    }

    [TestMethod]
    public async Task Reports_ValueLess_Boolean_Option_With_Reordered_Legacy_Enum()
    {
        const string source = """
            #nullable enable
            using System;
            using ModularPipelines.Attributes;
            using ModularPipelines.Options;

            namespace ModularPipelines.Attributes
            {
                public enum CliOptionValueArity
                {
                    Required = 3,
                    Optional = 11,
                    None = 17,
                }

                public sealed class CliArgumentAttribute : Attribute { }
                public sealed class CliFlagAttribute : Attribute
                {
                    public CliFlagAttribute(string name) { }
                }

                public sealed class CliOptionAttribute : Attribute
                {
                    public CliOptionAttribute(string name) { }
                    public CliOptionValueArity ValueArity { get; set; }
                }

                public sealed class CliSubCommandAttribute : Attribute { }
                public sealed class CliToolAttribute : Attribute
                {
                    public CliToolAttribute(string name) { }
                }
            }

            namespace ModularPipelines.Options
            {
                public abstract record CommandLineToolOptions;
            }

            [CliTool("tool")]
            public record Options : CommandLineToolOptions
            {
                [{|#0:CliOption("--force", ValueArity = CliOptionValueArity.None)|}]
                public bool? Force { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliPropertyAttributeAnalyzer.BooleanOptionDiagnosticId)
            .WithLocation(0)
            .WithArguments("Force");

        var test = new VerifyCS.Test
        {
            TestCode = source,
            ReferenceAssemblies = Net.Net100,
        };

        test.ExpectedDiagnostics.Add(expected);
        await test.RunAsync(CancellationToken.None);
    }

    [TestMethod]
    public async Task Accepts_Boolean_Option_That_Takes_A_Value()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("tool")]
            public record Options : CommandLineToolOptions
            {
                [CliOption("--tls-verify")]
                public bool? TlsVerify { get; init; }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Multiple_Cli_Attributes()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("tool")]
            public record Options : CommandLineToolOptions
            {
                [CliFlag("--force")]
                [{|#0:CliOption("--force")|}]
                public bool? Force { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliPropertyAttributeAnalyzer.MultipleAttributesDiagnosticId)
            .WithLocation(0)
            .WithArguments("Force");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }
}
