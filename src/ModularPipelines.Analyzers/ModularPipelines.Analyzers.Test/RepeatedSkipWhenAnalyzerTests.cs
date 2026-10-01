using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.RepeatedSkipWhenAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class RepeatedSkipWhenAnalyzerTests
{
    private const string Header = TestSourceConstants.StandardModuleHeaderWithExtensions;

    [TestMethod]
    public void Rule_Defaults_To_Info()
    {
        Assert.AreEqual(DiagnosticSeverity.Info, RepeatedSkipWhenAnalyzer.Rule.DefaultSeverity);
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

    private static string ModuleSource(string configure) => $$"""
        {{Header}}

        public class BuildModule : Module<List<string>>
        {
            {{configure}}

            {{TestSourceConstants.SimpleAsyncExecuteBody}}
        }
        """;
}
