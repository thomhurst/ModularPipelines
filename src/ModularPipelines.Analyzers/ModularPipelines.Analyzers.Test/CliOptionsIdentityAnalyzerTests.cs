using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    ModularPipelines.Analyzers.CliOptionsIdentityAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class CliOptionsIdentityAnalyzerTests
{
    [TestMethod]
    public async Task Reports_Cli_Attributes_Outside_Options_Hierarchy()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            public class {|#0:NotOptions|}
            {
                [CliFlag("--force")]
                public bool? Force { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliOptionsIdentityAnalyzer.InvalidOptionsBaseDiagnosticId)
            .WithLocation(0)
            .WithArguments("NotOptions");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Accepts_Derived_Tool_Override()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliTool("git")]
            public record BaseOptions : CommandLineToolOptions;

            [CliTool("docker")]
            public record DerivedOptions : BaseOptions;
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Static_Subcommand_Without_Tool()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [{|#0:CliSubCommand("run")|}]
            public record RunOptions : CommandLineToolOptions;
            """;

        var expected = VerifyCS.Diagnostic(CliOptionsIdentityAnalyzer.MissingToolDiagnosticId)
            .WithLocation(0)
            .WithArguments("RunOptions");

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Accepts_Runtime_Only_Options()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            public record RuntimeOptions : CommandLineToolOptions;
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }
}
