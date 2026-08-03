using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.AwaitThisAnalyzer,
    ModularPipelines.Analyzers.AwaitThisCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersAwaitThisUnitTests
{
    private const string BadModuleSourceAwaitThis = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        // This should trigger the analyzer
        {{|#0:await this|}}; // Preserve this explanation too
        return null!;
    }}
}}
";

    private const string FixedModuleSourceAwaitThis = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        // This should trigger the analyzer
        // Preserve this explanation too
        return null!;
    }}
}}
";

    private const string BadModuleSourceAwaitThisInMethod = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return await ExecuteCommand(context);
    }}

    private async Task<CommandResult> ExecuteCommand(IModuleContext context)
    {{
        // This should also trigger the analyzer
        {{|#0:await this|}};
        return null!;
    }}
}}
";

    private const string GoodModuleSourceNoAwaitThis = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        // This is fine - awaiting something else
        var otherModule = context.GetModule<Module2>();
        await otherModule;
        return null!;
    }}
}}

public class Module2 : Module<string>
{{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<string>(""Test"");
    }}
}}
";

    private const string NonModuleClassAwaitThis = @"
#nullable enable
using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ModularPipelines.Examples.Other;

public class NotAModule
{
    public async Task DoSomething()
    {
        // This should not trigger the analyzer since it's not in a module
        await this;
    }

    public TaskAwaiter GetAwaiter()
    {
        return Task.CompletedTask.GetAwaiter();
    }
}
";

    private const string GoodModuleSourceAwaitThisInOnAfterExecuteAsync = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<CommandResult>(null!);
    }}

    protected override async Task<ModuleResult<CommandResult>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<CommandResult> result,
        CancellationToken cancellationToken)
    {{
        // This should NOT trigger the analyzer - await this is allowed in OnAfterExecuteAsync
        return await this;
    }}
}}
";

    private const string BadModuleSourceAwaitThisAsEmbeddedStatement = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        if (DateTime.UtcNow.Ticks > 0)
            {{|#0:await this|}};

        return null!;
    }}
}}
";

    private const string FixedModuleSourceAwaitThisAsEmbeddedStatement = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        if (DateTime.UtcNow.Ticks > 0)
            ;

        return null!;
    }}
}}
";

    private const string BadModuleSourceAwaitThisInsideDirective = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
#region Self await
        await this;
#endregion
        return null!;
    }}
}}
";

    private const string BadModuleSourceAwaitThisAsLoopBody = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        while (!cancellationToken.IsCancellationRequested)
            await this;

        return null!;
    }}
}}
";

    private const string BadModuleSourceAwaitThisInsideLoopBlock = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        while (!cancellationToken.IsCancellationRequested)
        {{
            await this;
        }}

        return null!;
    }}
}}
";

    private const string BadModuleSourceAwaitThisAtGotoTarget = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
retry:
        await this;
        goto retry;
    }}
}}
";

    private const string BadModuleSourceAwaitThisInsideGotoCycle = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
retry:
        ;
        await this;
        goto retry;
    }}
}}
";

    private const string BadModuleSourceAwaitThisWithInactiveGoto = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
retry:
        await this;
#if RETRY
        goto retry;
#endif
        return null!;
    }}
}}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_AwaitThis_InExecuteAsync()
    {
        var expected = VerifyCS.Diagnostic(AwaitThisAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSourceAwaitThis, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_AwaitThis_InModuleMethod()
    {
        var expected = VerifyCS.Diagnostic(AwaitThisAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSourceAwaitThisInMethod, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_NoAwaitThis()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSourceNoAwaitThis);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_NotInModule()
    {
        await VerifyCS.VerifyAnalyzerAsync(NonModuleClassAwaitThis);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_AwaitThis_InOnAfterExecuteAsync()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSourceAwaitThisInOnAfterExecuteAsync);
    }

    [TestMethod]
    public async Task CodeFix_Removes_Standalone_Self_Await()
    {
        var expected = VerifyCS.Diagnostic(AwaitThisAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(BadModuleSourceAwaitThis, expected, FixedModuleSourceAwaitThis);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Embedded_Statement_Parent()
    {
        var expected = VerifyCS.Diagnostic(AwaitThisAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            BadModuleSourceAwaitThisAsEmbeddedStatement,
            expected,
            FixedModuleSourceAwaitThisAsEmbeddedStatement);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_Inside_Directives()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleSourceAwaitThisInsideDirective,
            AwaitThisAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Loop_Body()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleSourceAwaitThisAsLoopBody,
            AwaitThisAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_Inside_Loop_Block()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleSourceAwaitThisInsideLoopBlock,
            AwaitThisAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_At_Goto_Target()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleSourceAwaitThisAtGotoTarget,
            AwaitThisAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_Inside_Goto_Cycle()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleSourceAwaitThisInsideGotoCycle,
            AwaitThisAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Inactive_Goto_May_Create_Cycle()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleSourceAwaitThisWithInactiveGoto,
            AwaitThisAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_Inside_Goto_Case_Cycle()
    {
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<CommandResult>
{{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        switch (context)
        {{
            case null:
                await this;
                goto case null;
        }}
    }}
}}
";

        await VerifyCS.VerifyNoCodeFixAsync(source, AwaitThisAnalyzer.DiagnosticId);
    }
}
