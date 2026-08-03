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
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    private static readonly string QualifiedConsoleSource = CreateBadModuleSource(
        @"System.Console.WriteLine(""Done!"")");
    private static readonly string GlobalQualifiedConsoleSource = CreateBadModuleSource(
        @"global::System.Console.WriteLine(""Done!"")");
    private static readonly string NestedConsoleReceiverSource = CreateBadModuleSource(
        @"Console.Out.Encoding.GetBytes(""Done!"")");
    private static readonly string ConsoleInvocationReceiverSource = CreateBadModuleSource(
        @"{|#0:Console.OpenStandardInput()|}.ReadByte()",
        markDiagnostic: false);
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

    private const string DynamicConsoleArgumentSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        dynamic message = ""Done!"";
        {{|#0:Console.WriteLine(message)|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string UsingStaticConsoleSource = $@"
{TestSourceConstants.StandardUsings}
using static System.Console;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        {{|#0:WriteLine(""Done!"")|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string DynamicUsingStaticConsoleSource = $@"
{TestSourceConstants.StandardUsings}
using static System.Console;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        dynamic message = ""Done!"";
        {{|#0:WriteLine(message)|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    [TestMethod]
    public async Task AnalyzerReportsOnlyTheConsoleInvocationInAReceiverChain()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(ConsoleInvocationReceiverSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerReportsConsoleInvocationWithDynamicArgument()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(DynamicConsoleArgumentSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerReportsDynamicUsingStaticConsoleInvocation()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(DynamicUsingStaticConsoleSource, expected);
    }

    private const string CustomConsoleSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public static class CustomConsole
{{
    public static void WriteLine(string message)
    {{
    }}
}}

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        CustomConsole.WriteLine(""Done!"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string StaticLocalFunctionSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    private const string ShadowedContextSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        Action<int> write = context => Console.WriteLine(""Done!"");
        write(0);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string AliasedConsoleErrorWithEscapedContextSource = $@"
{TestSourceConstants.StandardUsings}
using C = System.Console;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        {{|#0:C.Error.WriteLine(""Failure!"")|}};
        return Task.FromResult<List<string>>([]);
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
    protected override Task<List<string>> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        @event.Logger.LogError(""{{Message}}"", (""Failure!"") ?? string.Empty);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string NullableConsoleMessageSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        string? message = null;
        {{|#0:Console.WriteLine(message)|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedNullableConsoleMessageSource = $@"
{TestSourceConstants.StandardUsings}
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        string? message = null;
        context.Logger.LogInformation(""{{Message}}"", (message) ?? string.Empty);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string LoggingNamespaceAliasSource = $@"
{TestSourceConstants.StandardUsings}
using Logging = Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        {{|#0:Console.WriteLine(""Done!"")|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedLoggingNamespaceAliasSource = $@"
{TestSourceConstants.StandardUsings}
using Logging = Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        context.Logger.LogInformation(""{{Message}}"", (""Done!"") ?? string.Empty);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string ConditionalLoggingUsingSource = $@"
#define LOGGING
{TestSourceConstants.StandardUsings}
#if LOGGING
using Microsoft.Extensions.Logging;
#endif

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        {{|#0:Console.WriteLine(""Done!"")|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedConditionalLoggingUsingSource = $@"
#define LOGGING
{TestSourceConstants.StandardUsings}
using Microsoft.Extensions.Logging;
#if LOGGING
using Microsoft.Extensions.Logging;
#endif

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        context.Logger.LogInformation(""{{Message}}"", (""Done!"") ?? string.Empty);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string SuppressedNullConsoleMessageSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        {{|#0:Console.WriteLine((string)null!)|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedSuppressedNullConsoleMessageSource = $@"
{TestSourceConstants.StandardUsings}
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        context.Logger.LogInformation(""{{Message}}"", ((string)null!) ?? string.Empty);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string DirectiveInConsoleReceiverSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        Console
#if true
            .WriteLine(""Done!"")
#endif
            ;
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string NullableConsoleMessageWithCommentSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        string? message = null;
        {{|#0:Console.WriteLine(/* explanation */ message)|}};
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedNullableConsoleMessageWithCommentSource = $@"
{TestSourceConstants.StandardUsings}
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        string? message = null;
        context.Logger.LogInformation(""{{Message}}"", /* explanation */ (message) ?? string.Empty);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string ConflictingLoggerExtensionSource = $@"
{TestSourceConstants.StandardUsings}

namespace AnalyzerExamples;

public static class CustomLoggerExtensions
{{
    public static void LogInformation(
        this ModularPipelines.Logging.IModuleLogger logger,
        string message,
        string value)
    {{
    }}
}}

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        Console.WriteLine(""Done!"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private static string CreateFixedModuleSource(string loggerCall) => $@"
{TestSourceConstants.StandardUsings}
using Microsoft.Extensions.Logging;

namespace AnalyzerExamples;

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    public async Task AnalyzerIsTriggered_When_Using_QualifiedConsole()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(QualifiedConsoleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_GlobalQualifiedConsole()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(GlobalQualifiedConsoleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_UsingStaticConsole()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(UsingStaticConsoleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_ConsoleReceiverIsNested()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(NestedConsoleReceiverSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_Using_CustomConsoleType()
    {
        await VerifyCS.VerifyAnalyzerAsync(CustomConsoleSource);
    }

    [TestMethod]
    public async Task CodeFix_Replaces_Console_WriteLine_With_Logger()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        var fixedSource = CreateFixedModuleSource(
            @"context.Logger.LogInformation(""{Message}"", (""Done!"") ?? string.Empty)");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Replaces_Awaited_Console_WriteLine_With_Logger()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        var fixedSource = CreateFixedModuleSource(
            @"context.Logger.LogInformation(""{Message}"", (""Done!"") ?? string.Empty)");

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource5, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Passes_Brace_Containing_Text_As_Logging_Value()
    {
        var source = CreateBadModuleSource(
            @"Console.WriteLine(""Status: {pending}"")");
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        var fixedSource = CreateFixedModuleSource(
            @"context.Logger.LogInformation(""{Message}"", (""Status: {pending}"") ?? string.Empty)");

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
    public async Task CodeFix_Is_Not_Offered_When_Context_Is_Shadowed()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ShadowedContextSource,
            ConsoleUseAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Console_Invocation_Contains_Directives()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            DirectiveInConsoleReceiverSource,
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
    public async Task CodeFix_Adds_Logging_Import_When_Only_Alias_Exists()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            LoggingNamespaceAliasSource,
            expected,
            FixedLoggingNamespaceAliasSource);
    }

    [TestMethod]
    public async Task CodeFix_Adds_Unconditional_Logging_Import()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            ConditionalLoggingUsingSource,
            expected,
            FixedConditionalLoggingUsingSource);
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

    [TestMethod]
    public async Task CodeFix_Coalesces_Runtime_Null_When_Flow_State_Is_NotNull()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            SuppressedNullConsoleMessageSource,
            expected,
            FixedSuppressedNullConsoleMessageSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Nullable_Message_Comment()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            NullableConsoleMessageWithCommentSource,
            expected,
            FixedNullableConsoleMessageWithCommentSource);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Logging_Extension_Would_Be_Shadowed()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ConflictingLoggerExtensionSource,
            ConsoleUseAnalyzer.DiagnosticId);
    }
}
