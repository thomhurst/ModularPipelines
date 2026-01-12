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
    {{|#0:protected override Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return ExecuteCommand(context);
    }}|}}

    private async Task<CommandResult?> ExecuteCommand(IModuleContext context)
    {{
        return await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions(""git""));
    }}
}}
";

    private const string BadModuleSource2 = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    {{|#0:protected override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        if (1 + ""n"" == ""1n"")
        {{
            return ExecuteCommand(context);
        }}

        return Task.FromResult<string?>(""Foo!"");
    }}|}}

    private async Task<string?> ExecuteCommand(IModuleContext context)
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
    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return await ExecuteCommand(context);
    }}

    private async Task<CommandResult?> ExecuteCommand(IModuleContext context)
    {{
        return await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions(""git""));
    }}
}}
";

    private const string GoodModuleSource2 = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    protected override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<string?>(""Foo"");
    }}
}}
";

    private const string GoodModuleSource3 = $@"
{TestSourceConstants.StandardModuleHeaderWithExtensions}

public class Module1 : Module<string>
{{
    protected override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return ""Foo"".AsTask<string?>();
    }}
}}
";

    private const string BadModuleSource2Fixed = $@"
{TestSourceConstants.StandardModuleHeaderWithOptions}

public class Module1 : Module<string>
{{
    {{|#0:protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        if (1 + ""n"" == ""1n"")
        {{
            return await ExecuteCommand(context);
        }}

        return ""Foo!"";
    }}|}}

    private async Task<string?> ExecuteCommand(IModuleContext context)
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
    public async Task AnalyzerIsNotTriggered_When_AsTaskExtension()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource3);
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
}