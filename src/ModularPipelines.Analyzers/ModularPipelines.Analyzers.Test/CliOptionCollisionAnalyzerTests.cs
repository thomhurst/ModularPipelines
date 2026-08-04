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
    public async Task Accepts_New_Property_That_Hides_Base_Property()
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
                [CliFlag("--overwrite", ShortForm = "-o")]
                public new bool? Output { get; init; }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Duplicate_Switch_From_Different_Derived_Property()
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
                public bool? Overwrite { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliOptionCollisionAnalyzer.DuplicateSwitchDiagnosticId)
            .WithLocation(0)
            .WithArguments("-o", "BaseOptions.Output", "DerivedOptions.Overwrite");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Duplicate_Switch_Inherited_By_Override()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("tool")]
            public record BaseOptions : CommandLineToolOptions
            {
                [CliOption("--output")]
                public virtual string? Output { get; init; }
            }

            public record DerivedOptions : BaseOptions
            {
                public override string? Output { get; init; }

                [{|#0:CliFlag("--output")|}]
                public bool? Overwrite { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliOptionCollisionAnalyzer.DuplicateSwitchDiagnosticId)
            .WithLocation(0)
            .WithArguments("--output", "DerivedOptions.Output", "DerivedOptions.Overwrite");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Accepts_Static_And_WriteOnly_Properties_Omitted_From_Command_Model()
    {
        var source = CreateOptionsSource("""
            [CliFlag("--static")]
            public static bool? Static { get; set; }

            [CliOption("--static")]
            public string? Instance { get; init; }

            [CliFlag("--write-only")]
            public bool? WriteOnly { set { } }

            [CliOption("--write-only")]
            public string? Readable { get; init; }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Accepts_Duplicate_Argument_Position_In_Same_Phase()
    {
        var source = CreateOptionsSource("""
            [CliArgument(0)]
            public string? Input { get; init; }

            [CliArgument(0, Phase = CommandLinePhase.Passthrough)]
            public string? Output { get; init; }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
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
