using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.AsyncModuleAnalyzer,
    ModularPipelines.Analyzers.AsyncModuleCodeFixProvider>;
namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersAsyncModulesUnitTests
{
    private const string BadModuleSource = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<CommandResult>
{
    {|#0:protected override Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return ExecuteCommand(context);
    }|}

    private async Task<CommandResult?> ExecuteCommand(IPipelineContext context)
    {
        return await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions(""git""));
    }
}
";

    private const string GoodModuleSource = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(context);
    }

    private async Task<CommandResult?> ExecuteCommand(IPipelineContext context)
    {
        return await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions(""git""));
    }
}
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
    public async Task CodeFixWorks()
    {
        // if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        // {
        //     // This fails on Linux only due to different line endings
        //     // Is there a way around that?
        //     return;
        // }

        var expected = VerifyCS.Diagnostic(AsyncModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource, expected, GoodModuleSource);
    }
}