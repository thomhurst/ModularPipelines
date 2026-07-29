using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.ConsoleUseAnalyzer,
    ModularPipelines.Analyzers.ConsoleUseCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersConsoleUnitTests
{
    private static string CreateBadModuleSource(
        string consoleCall,
        bool isAsync = false,
        bool markDiagnostic = true) => $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);

        {(isAsync ? "await " : "")}{(markDiagnostic ? $"{{|#0:{consoleCall}|}}" : consoleCall)};

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
    private static readonly string NamedArgumentSource = CreateBadModuleSource(
        @"Console.WriteLine(value: ""Done!"")",
        markDiagnostic: false);

    private const string StaticLocalFunctionSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        static void WriteMessage()
        {{
            Console.WriteLine(""Done!"");
        }}

        await Task.Delay(1, cancellationToken);
        WriteMessage();
        return new List<string>();
    }}
}}
";

    private const string AliasedConsoleErrorWithEscapedContextSource = $@"
{TestSourceConstants.StandardUsings}
using C = System.Console;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>?> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        {{|#0:C.Error.WriteLine(""Failure!"")|}};
        return Task.FromResult<List<string>?>([]);
    }}
}}
";

    private const string FixedAliasedConsoleErrorWithEscapedContextSource = $@"
{TestSourceConstants.StandardUsings}
using C = System.Console;
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>?> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        @event.Logger.LogError(""Failure!"");
        return Task.FromResult<List<string>?>([]);
    }}
}}
";

    private static string CreateFixedModuleSource(string loggerCall) => $@"
{TestSourceConstants.StandardUsings}
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);

        {loggerCall};

        return new List<string>();
    }}
}}
";

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

    [TestMethod]
    public async Task CodeFix_Replaces_Console_WriteLine_With_Logger()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        var fixedSource = CreateFixedModuleSource(@"context.Logger.LogInformation(""Done!"")");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Replaces_Awaited_Console_WriteLine_With_Logger()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        var fixedSource = CreateFixedModuleSource(@"context.Logger.LogInformation(""Done!"")");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource5, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_In_Static_Local_Function()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            StaticLocalFunctionSource,
            ConsoleUseAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Named_Console_Argument()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            NamedArgumentSource,
            ConsoleUseAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Uses_Error_Level_And_Preserves_Escaped_Context()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            AliasedConsoleErrorWithEscapedContextSource,
            expected,
            FixedAliasedConsoleErrorWithEscapedContextSource);
    }
}
