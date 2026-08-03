using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    ModularPipelines.Analyzers.CliOptionCollisionAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CliOptionCollisionAnalyzerTests
{
    [TestMethod]
    public async Task Reports_Duplicate_Long_Switch()
    {
        var source = CreateOptionsSource("""
            [CliFlag("--verbose")]
            public bool? Verbose { get; init; }

            [{|#0:CliOption("--verbose")|}]
            public string? VerboseValue { get; init; }
            """);

        var expected = VerifyCS.Diagnostic(CliOptionCollisionAnalyzer.DuplicateSwitchDiagnosticId)
            .WithLocation(0)
            .WithArguments("--verbose", "Options.Verbose", "Options.VerboseValue");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Duplicate_Short_Switch_From_New_Property()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("tool")]
            public record BaseOptions : CommandLineToolOptions
            {
                [CliOption("--output", ShortForm = "-o")]
                public string? Output { get; init; }
            }

            public record DerivedOptions : BaseOptions
            {
                [{|#0:CliFlag("--overwrite", ShortForm = "-o")|}]
                public new bool? Output { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliOptionCollisionAnalyzer.DuplicateSwitchDiagnosticId)
            .WithLocation(0)
            .WithArguments("-o", "BaseOptions.Output", "DerivedOptions.Output");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Duplicate_Argument_Position_In_Same_Phase()
    {
        var source = CreateOptionsSource("""
            [CliArgument(0)]
            public string? Input { get; init; }

            [{|#0:CliArgument(0, Phase = CommandLinePhase.Passthrough)|}]
            public string? Output { get; init; }
            """);

        var expected = VerifyCS.Diagnostic(CliOptionCollisionAnalyzer.DuplicateArgumentPositionDiagnosticId)
            .WithLocation(0)
            .WithArguments(0, "Options.Input", "Options.Output");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Accepts_Same_Argument_Position_In_Different_Phases()
    {
        var source = CreateOptionsSource("""
            [CliArgument(0, Phase = CommandLinePhase.Normal)]
            public string? Input { get; init; }

            [CliArgument(0)]
            public string? Output { get; init; }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    private static string CreateOptionsSource(string properties) => $$"""
        {{TestSourceConstants.StandardUsingsWithOptions}}

        [CliTool("tool")]
        public record Options : CommandLineToolOptions
        {
            {{properties}}
        }
        """;
}
