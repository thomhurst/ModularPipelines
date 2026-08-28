using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.AsyncModuleAnalyzer,
    ModularPipelines.Analyzers.AsyncModuleCodeFixProvider>;
namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersAsyncModulesUnitTests
{
    private const string BadModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    {{|#0:protected override Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return ExecuteCommand(context);
    }}|}}

    private async Task<CommandResult> ExecuteCommand(IModuleContext context)
    {{
        return (await context.Shell.RunAsync(""git"", []))!;
    }}
}}
";

    private const string BadModuleSource2 = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    {{|#0:protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        if (1 + ""n"" == ""1n"")
        {{
            return ExecuteCommand(context);
        }}

        return Task.FromResult<string>(""Foo!"");
    }}|}}

    private async Task<string> ExecuteCommand(IModuleContext context)
    {{
        await Task.Yield();
        return ""Foo!"";
    }}
}}
";

    private const string GoodModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return await ExecuteCommand(context);
    }}

    private async Task<CommandResult> ExecuteCommand(IModuleContext context)
    {{
        return (await context.Shell.RunAsync(""git"", []))!;
    }}
}}
";

    private const string GoodModuleSource2 = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<string>(""Foo"");
    }}
}}
";

    private const string ExpressionBodiedModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    {{|#0:protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => ExecuteCommand(context);|}}

    private static Task<string> ExecuteCommand(IModuleContext context)
        => Task.FromResult(""Foo"");
}}
";

    private const string FixedExpressionBodiedModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    {{|#0:protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => await ExecuteCommand(context);|}}

    private static Task<string> ExecuteCommand(IModuleContext context)
        => Task.FromResult(""Foo"");
}}
";

    private const string BadModuleSource2Fixed = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    {{|#0:protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        if (1 + ""n"" == ""1n"")
        {{
            return await ExecuteCommand(context);
        }}

        return ""Foo!"";
    }}|}}

    private async Task<string> ExecuteCommand(IModuleContext context)
    {{
        await Task.Yield();
        return ""Foo!"";
    }}
}}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Not_Async()
    {
        var expected = VerifyCS.Diagnostic(AsyncModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_Async()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_TaskFromResult()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource2);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_For_Returns_In_Lambdas_Or_Local_Functions()
    {
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        Func<string> getLambdaValue = () =>
        {{
            return ""lambda"";
        }};

        string GetLocalValue()
        {{
            return ""local"";
        }}

        return Task.FromResult(getLambdaValue() + GetLocalValue());
    }}
}}
";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_Expression_Body_Uses_TaskFromResult()
    {
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult(""Foo"");
}}
";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task CodeFixWorks()
    {
        var expected = VerifyCS.Diagnostic(AsyncModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource, expected, GoodModuleSource);
    }

    [TestMethod]
    public async Task CodeFixWorks_With_Mixed_TaskFromResult_And_Actual_Async()
    {
        var expected = VerifyCS.Diagnostic(AsyncModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource2, expected, BadModuleSource2Fixed);
    }

    [TestMethod]
    public async Task CodeFixWorks_For_Expression_Bodied_Method()
    {
        var expected = VerifyCS.Diagnostic(AsyncModuleAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            ExpressionBodiedModuleSource,
            expected,
            FixedExpressionBodiedModuleSource);
    }

    [TestMethod]
    public async Task CodeFixParenthesizesConditionalExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "context is null ? ExecuteCommand() : ExecuteCommand()",
            "await (context is null ? ExecuteCommand() : ExecuteCommand())");
    }

    [TestMethod]
    public async Task CodeFixParenthesizesSwitchExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "context switch { null => ExecuteCommand(), _ => ExecuteCommand() }",
            "await (context switch { null => ExecuteCommand(), _ => ExecuteCommand() })");
    }

    [TestMethod]
    public async Task CodeFixParenthesizesCoalescingExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "PendingTask ?? ExecuteCommand()",
            "await (PendingTask ?? ExecuteCommand())");
    }

    [TestMethod]
    public async Task CodeFixParenthesizesAssignmentExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "PendingTask = ExecuteCommand()",
            "await (PendingTask = ExecuteCommand())");
    }

    [TestMethod]
    public async Task CodeFixPreservesThrowExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "throw new InvalidOperationException()",
            "throw new InvalidOperationException()");
    }

    [TestMethod]
    public async Task CodeFixPreservesTargetTypedNullExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "null",
            "null",
            suppressNullReturnWarning: true);
    }

    [TestMethod]
    public async Task CodeFixPreservesTargetTypedDefaultExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "default",
            "default",
            suppressNullReturnWarning: true);
    }

    [TestMethod]
    public async Task CodeFixPreservesTargetTypedConditionalExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "context is null ? null : throw new InvalidOperationException()",
            "context is null ? null : throw new InvalidOperationException()",
            suppressNullReturnWarning: true);
    }

    [TestMethod]
    public async Task CodeFixPreservesTargetTypedSwitchExpressionBody()
    {
        await VerifyExpressionBodiedCodeFixAsync(
            "context switch { null => null, _ => default }",
            "context switch { null => null, _ => default }",
            suppressNullReturnWarning: true);
    }

    private static async Task VerifyExpressionBodiedCodeFixAsync(
        string expression,
        string fixedExpression,
        bool suppressNullReturnWarning = false)
    {
        var nullableWarningDirective = suppressNullReturnWarning
            ? "#pragma warning disable CS8603"
            : string.Empty;
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}
{nullableWarningDirective}

public class Module1 : Module<string>
{{
    {{|#0:protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => {expression};|}}

    private static Task<string>? PendingTask {{ get; set; }}

    private static Task<string> ExecuteCommand()
        => Task.FromResult(""Foo"");
}}
";
        var fixedSource = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}
{nullableWarningDirective}

public class Module1 : Module<string>
{{
    {{|#0:protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => {fixedExpression};|}}

    private static Task<string>? PendingTask {{ get; set; }}

    private static Task<string> ExecuteCommand()
        => Task.FromResult(""Foo"");
}}
";
        var expected = VerifyCS.Diagnostic(AsyncModuleAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}
