using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ConsoleUseAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersConsoleUnitTests
{
    private static string CreateBadModuleSource(string consoleCall, bool isAsync = false) => $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);

        {(isAsync ? "await " : "")}{{|#0:{consoleCall}|}};

        return new List<string>();
    }}
}}
";

    private static readonly string BadModuleSource = CreateBadModuleSource(@"Console.WriteLine(""Done!"")");
    private static readonly string BadModuleSource2 = CreateBadModuleSource(@"Console.Write(""Done!"")");
    private static readonly string BadModuleSource3 = CreateBadModuleSource(@"Console.Out.Write(""Done!"")");
    private static readonly string BadModuleSource4 = CreateBadModuleSource(@"Console.Out.WriteLine(""Done!"")");
    private static readonly string BadModuleSource5 = CreateBadModuleSource(@"Console.Out.WriteLineAsync(""Done!"")", isAsync: true);
    private static readonly string BadModuleSource6 = CreateBadModuleSource(@"Console.Out.Dispose()");

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console2()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource2, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console3()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource3, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console4()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource4, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console5()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource5, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console6()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource6, expected);
    }
}