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
    
    private const string BadModuleSource2 = @"
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

public class Module1 : Module<string>
{
    {|#0:protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        if (1 + ""n"" == ""1n"")
        {
            return ExecuteCommand(context);
        }

        return Task.FromResult<string?>(""Foo!"");
    }|}

    private async Task<string?> ExecuteCommand(IPipelineContext context)
    {
        await Task.Yield();
        return ""Foo!"";
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
    
    private const string GoodModuleSource2 = @"
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

public class Module1 : Module<string>
{
    protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(""Foo"");
    }
}
";
    
    private const string GoodModuleSource3 = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<string>
{
    protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return ""Foo"".AsTask<string?>();
    }
}
";
    
    private const string BadModuleSource2Fixed = @"
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

public class Module1 : Module<string>
{
    {|#0:protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        if (1 + ""n"" == ""1n"")
        {
            return await ExecuteCommand(context);
        }

        return ""Foo!"";
    }|}

    private async Task<string?> ExecuteCommand(IPipelineContext context)
    {
        await Task.Yield();
        return ""Foo!"";
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