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
    private static readonly string NonTerminatingWriteSource =
        CreateBadModuleSource(@"Console.Write(""Done!"")", markDiagnostic: false);
    private static readonly string NonTerminatingWriteAsyncSource =
        CreateBadModuleSource(
            @"Console.Out.WriteAsync(""Done!"")",
            isAsync: true,
            markDiagnostic: false);
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
        @event.Logger.LogError(""{{Message}}"", ""Failure!"");
        return Task.FromResult<List<string>?>([]);
    }}
}}
";

    private const string NullableConsoleMessageSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        string? message = null;
        {{|#0:Console.WriteLine(message)|}};
        return Task.FromResult<List<string>?>([]);
    }}
}}
";

    private const string FixedNullableConsoleMessageSource = $@"
{TestSourceConstants.StandardUsings}
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        string? message = null;
        context.Logger.LogInformation(""{{Message}}"", (message) ?? string.Empty);
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
        var fixedSource = CreateFixedModuleSource(
            @"context.Logger.LogInformation(""{Message}"", ""Done!"")");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Replaces_Awaited_Console_WriteLine_With_Logger()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        var fixedSource = CreateFixedModuleSource(
            @"context.Logger.LogInformation(""{Message}"", ""Done!"")");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource5, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Passes_Brace_Containing_Text_As_Logging_Value()
    {
        var source = CreateBadModuleSource(
            @"Console.WriteLine(""Status: {pending}"")");
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        var fixedSource = CreateFixedModuleSource(
            @"context.Logger.LogInformation(""{Message}"", ""Status: {pending}"")");

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
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
    public async Task CodeFix_Is_Not_Offered_For_Non_Terminating_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            NonTerminatingWriteSource,
            ConsoleUseAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Non_Terminating_WriteAsync()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            NonTerminatingWriteAsyncSource,
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

    [TestMethod]
    public async Task CodeFix_Coalesces_Nullable_Message_To_Empty_String()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            NullableConsoleMessageSource,
            expected,
            FixedNullableConsoleMessageSource);
    }
}
