using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    ModularPipelines.Analyzers.CliPropertyAttributeAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CliPropertyAttributeAnalyzerTests
{
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
    public async Task Accepts_Nullable_Boolean_And_Integer_Flags()
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
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_ValueLess_Boolean_Option()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

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

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
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
