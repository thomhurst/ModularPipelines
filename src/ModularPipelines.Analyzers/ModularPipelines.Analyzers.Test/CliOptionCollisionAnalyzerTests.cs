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

    [TestMethod]
    public async Task Accepts_Named_Arguments_With_Same_Position()
    {
        var source = CreateOptionsSource("""
            [CliArgument(0, Name = "<SOURCE>")]
            public string? Source { get; init; }

            [CliArgument(0, Name = "<DESTINATION>")]
            public string? Destination { get; init; }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Duplicate_Position_With_Reordered_Legacy_Enums()
    {
        const string source = """
            #nullable enable
            using System;
            using ModularPipelines.Attributes;
            using ModularPipelines.Options;

            namespace ModularPipelines.Attributes
            {
                public enum CommandLinePhase
                {
                    Normal = 11,
                    Passthrough = 17,
                }

                public enum ArgumentPlacement
                {
                    BeforeOptions = 19,
                    AfterOptions = 23,
                }

                public sealed class CliArgumentAttribute : Attribute
                {
                    public CliArgumentAttribute(int position) { }
                    public string? Name { get; set; }
                    public CommandLinePhase Phase { get; set; } = CommandLinePhase.Passthrough;
                    public ArgumentPlacement Placement { get; set; } = ArgumentPlacement.AfterOptions;
                }

                public sealed class CliFlagAttribute : Attribute
                {
                    public CliFlagAttribute(string name) { }
                    public string? ShortForm { get; set; }
                }

                public sealed class CliOptionAttribute : Attribute
                {
                    public CliOptionAttribute(string name) { }
                    public string? ShortForm { get; set; }
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
                [CliArgument(0)]
                public string? Input { get; init; }

                [{|#0:CliArgument(0, Phase = CommandLinePhase.Passthrough, Placement = ArgumentPlacement.AfterOptions)|}]
                public string? Output { get; init; }
            }
            """;

        var expected = VerifyCS.Diagnostic(CliOptionCollisionAnalyzer.DuplicateArgumentPositionDiagnosticId)
            .WithLocation(0)
            .WithArguments(0, "Options.Input", "Options.Output");

        var test = new VerifyCS.Test
        {
            TestCode = source,
            ReferenceAssemblies = Net.Net100,
        };

        test.ExpectedDiagnostics.Add(expected);
        await test.RunAsync(CancellationToken.None);
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
