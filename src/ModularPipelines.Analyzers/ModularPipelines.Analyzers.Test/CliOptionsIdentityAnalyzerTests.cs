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
    public async Task Accepts_Subcommand_With_Runtime_Tool()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            [CliSubCommand("run")]
            public record RunOptions : CommandLineToolOptions
            {
                public RunOptions()
                {
                    Tool = "runtime-tool";
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Cli_Property_Attributes_On_Interfaces()
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsingsWithOptions}}

            public interface {|#0:IOptions|}
            {
                [CliFlag("--force")]
                bool? Force { get; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliOptionsIdentityAnalyzer.InvalidOptionsBaseDiagnosticId)
            .WithLocation(0)
            .WithArguments("IOptions");

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
