using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    ModularPipelines.Analyzers.AwaitThisAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersAwaitThisUnitTests
{
    private const string BadModuleSourceAwaitThis = @"
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
        // This should trigger the analyzer
        {|#0:await this|};
        return null;
    }
}
";

    private const string BadModuleSourceAwaitThisInMethod = @"
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
        // This should also trigger the analyzer
        {|#0:await this|};
        return null;
    }
}
";

    private const string GoodModuleSourceNoAwaitThis = @"
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
        // This is fine - awaiting something else
        var otherModule = GetModule<Module2>();
        await otherModule;
        return null;
    }
}

public class Module2 : Module<string>
{
    protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(""Test"");
    }
}
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

    private const string GoodModuleSourceAwaitThisInOnAfterExecute = @"
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
    protected override Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<CommandResult?>(null);
    }

    protected override async Task OnAfterExecute(IPipelineContext context)
    {
        // This should NOT trigger the analyzer - await this is allowed in OnAfterExecute
        var result = await this;
    }
}
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
    public async Task AnalyzerIsNotTriggered_When_AwaitThis_InOnAfterExecute()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSourceAwaitThisInOnAfterExecute);
    }
}