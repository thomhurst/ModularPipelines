using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.RepeatedSkipWhenAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class RepeatedSkipWhenAnalyzerTests
{
    private const string Header = TestSourceConstants.StandardModuleHeaderWithExtensions;

    [TestMethod]
    public void Rule_Defaults_To_Warning()
    {
        Assert.AreEqual(DiagnosticSeverity.Warning, RepeatedSkipWhenAnalyzer.Rule.DefaultSeverity);
    }

    [TestMethod]
    public async Task Reports_Repeated_WithSkipWhen_In_Fluent_Chain()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module) => module
                    .WithSkipWhen(_ => SkipDecision.Skip("first"))
                    .WithTimeout(TimeSpan.FromMinutes(1))
                    .{|#0:WithSkipWhen|}(_ => false, "second")
                    .{|#1:WithSkipWhen|}(_ => SkipDecision.DoNotSkip);
            """);

        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(1));
    }

    [TestMethod]
    public async Task Reports_Repeated_WithSkipWhen_In_Separate_Statements_On_Same_Builder()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module)
                {
                    module.WithSkipWhen(_ => SkipDecision.Skip("first"));
                    module.WithTimeout(TimeSpan.FromMinutes(1)).{|#0:WithSkipWhen|}(async (_, _) =>
                    {
                        await Task.Yield();
                        return SkipDecision.DoNotSkip;
                    });
                }
            """);

        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(0));
    }

    [TestMethod]
    public async Task Does_Not_Report_Single_WithSkipWhen()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module) => module
                    .WithSkipWhen(_ => SkipDecision.Skip("only"))
                    .WithSkipWhenAll(
                        _ => SkipDecision.Skip("first"),
                        _ => SkipDecision.Skip("second"));
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_WithSkipWhen_On_Different_Builders()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder first, ModuleConfigurationBuilder second)
                {
                    first.WithSkipWhen(_ => SkipDecision.Skip("first"));
                    second.WithSkipWhen(_ => SkipDecision.Skip("second"));
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_WithSkipWhen_In_Separate_Members()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static ModuleConfigurationBuilder First(this ModuleConfigurationBuilder module) =>
                    module.WithSkipWhen(_ => SkipDecision.Skip("first"));

                public static ModuleConfigurationBuilder Second(this ModuleConfigurationBuilder module) =>
                    module.WithSkipWhen(_ => SkipDecision.Skip("second"));
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Repeated_WithSkipWhen_Through_Local_Aliases()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module)
                {
                    var configured = module.WithSkipWhen(_ => SkipDecision.Skip("first"));
                    configured.{|#0:WithSkipWhen|}(_ => SkipDecision.Skip("second"));
                    var alias = module;
                    alias.{|#1:WithSkipWhen|}(_ => SkipDecision.Skip("third"));
                }
            """);

        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(1));
    }

    [TestMethod]
    public async Task Does_Not_Follow_Reassigned_Local()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder first, ModuleConfigurationBuilder second)
                {
                    var builder = first;
                    builder.WithSkipWhen(_ => SkipDecision.Skip("first"));
                    builder = second;
                    builder.WithSkipWhen(_ => SkipDecision.Skip("second"));
                    first.WithTimeout(TimeSpan.FromMinutes(1));
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Repeated_WithSkipWhen_On_Reassigned_Local_Between_Writes()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder first, ModuleConfigurationBuilder second)
                {
                    var builder = first;
                    builder.WithSkipWhen(_ => SkipDecision.Skip("first a"));
                    builder.{|#0:WithSkipWhen|}(_ => SkipDecision.Skip("first b"));
                    builder = second;
                    builder.WithSkipWhen(_ => SkipDecision.Skip("second a"));
                    if (Environment.ProcessorCount > 1)
                    {
                        builder.{|#1:WithSkipWhen|}(_ => SkipDecision.Skip("second b"));
                    }
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(1));
    }

    [TestMethod]
    public async Task Does_Not_Report_WithSkipWhen_In_Switch_Sections_Not_Joined_By_Goto()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module)
                {
                    switch (Environment.ProcessorCount)
                    {
                        case 1:
                            module.WithSkipWhen(_ => SkipDecision.Skip("one"));
                            switch (Environment.TickCount)
                            {
                                case 0:
                                    goto default;
                                default:
                                    break;
                            }

                            break;
                        case 2:
                            module.WithSkipWhen(_ => SkipDecision.Skip("two"));
                            break;
                        case 3:
                            goto case 2;
                        default:
                            break;
                    }
                }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Follow_Local_Written_By_Deconstruction_Or_Lambda()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder first, ModuleConfigurationBuilder second)
                {
                    var deconstructed = first;
                    (deconstructed, _) = (second, 0);
                    first.WithSkipWhen(_ => SkipDecision.Skip("first"));
                    deconstructed.WithSkipWhen(_ => SkipDecision.Skip("deconstructed"));

                    var captured = second;
                    Action reset = () => captured = first;
                    reset();
                    second.WithSkipWhen(_ => SkipDecision.Skip("second"));
                    captured.WithSkipWhen(_ => SkipDecision.Skip("captured"));
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_WithSkipWhen_After_Conditional_Call()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module)
                {
                    if (Environment.ProcessorCount > 1)
                    {
                        module.WithSkipWhen(_ => SkipDecision.Skip("conditional"));
                    }

                    foreach (var name in new[] { "a", "b" })
                    {
                        module.{|#0:WithSkipWhen|}(_ => SkipDecision.Skip(name));
                    }
                }
            """);

        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(0));
    }

    [TestMethod]
    public async Task Does_Not_Report_WithSkipWhen_Within_One_Branching_Statement()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder builder, ModuleConfigurationBuilder[] builders)
                {
                    for (var i = 0; i < builders.Length; i++)
                    {
                        builder = builders[i];
                        if (i == 0)
                        {
                            builder.WithSkipWhen(_ => SkipDecision.Skip("first"));
                        }
                        else
                        {
                            builder.WithSkipWhen(_ => SkipDecision.Skip("other"));
                        }
                    }
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_WithSkipWhen_Across_Parameter_Reassignment()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder builder, ModuleConfigurationBuilder next)
                {
                    builder.WithSkipWhen(_ => SkipDecision.Skip("first"));
                    builder = next;
                    builder.WithSkipWhen(_ => SkipDecision.Skip("second"));
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Repeated_WithSkipWhen_Across_Self_Assignment()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder builder, ModuleConfigurationBuilder first)
                {
                    builder = builder.WithSkipWhen(_ => SkipDecision.Skip("parameter a"));
                    builder.{|#0:WithSkipWhen|}(_ => SkipDecision.Skip("parameter b"));

                    var local = first;
                    local = local.WithTimeout(TimeSpan.FromMinutes(1)).WithSkipWhen(_ => SkipDecision.Skip("local a"));
                    local.{|#1:WithSkipWhen|}(_ => SkipDecision.Skip("local b"));
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(1));
    }

    [TestMethod]
    public async Task Does_Not_Report_WithSkipWhen_Across_Assignment_From_Another_Builder_Chain()
    {
        var source = $$"""
            {{Header}}

            public static class SkipConfiguration
            {
                public static void Configure(ModuleConfigurationBuilder builder, ModuleConfigurationBuilder next)
                {
                    builder = builder.WithSkipWhen(_ => SkipDecision.Skip("first"));
                    builder = next.WithTimeout(TimeSpan.FromMinutes(1));
                    builder.WithSkipWhen(_ => SkipDecision.Skip("second"));
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_WithSkipWhen_In_Mutually_Exclusive_Branches()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module)
                {
                    if (Environment.ProcessorCount > 1)
                    {
                        module.WithSkipWhen(_ => SkipDecision.Skip("if"));
                    }
                    else if (Environment.ProcessorCount > 0)
                    {
                        module.WithSkipWhen(_ => SkipDecision.Skip("else if"));
                    }
                    else
                    {
                        module.WithSkipWhen(_ => SkipDecision.Skip("else"));
                    }

                    switch (Environment.ProcessorCount)
                    {
                        case 1:
                            module.WithTimeout(TimeSpan.FromMinutes(1));
                            break;
                        default:
                            module.WithTimeout(TimeSpan.FromMinutes(2));
                            break;
                    }

                    _ = Environment.ProcessorCount > 1
                        ? module.WithTimeout(TimeSpan.FromMinutes(1))
                        : module.WithTimeout(TimeSpan.FromMinutes(2));
                }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_WithSkipWhen_After_Branches()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module)
                {
                    switch (Environment.ProcessorCount)
                    {
                        case 1:
                            module.WithSkipWhen(_ => SkipDecision.Skip("one"));
                            break;
                        default:
                            module.WithSkipWhen(_ => SkipDecision.Skip("many"));
                            break;
                    }

                    _ = Environment.ProcessorCount > 1
                        ? module.{|#0:WithSkipWhen|}(_ => SkipDecision.Skip("true"))
                        : module.{|#1:WithSkipWhen|}(_ => SkipDecision.Skip("false"));
                }
            """);

        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(RepeatedSkipWhenAnalyzer.DiagnosticId).WithLocation(1));
    }

    private static string ModuleSource(string configure) => $$"""
        {{Header}}

        public class BuildModule : Module<List<string>>
        {
            {{configure}}

            {{TestSourceConstants.SimpleAsyncExecuteBody}}
        }
        """;
}
