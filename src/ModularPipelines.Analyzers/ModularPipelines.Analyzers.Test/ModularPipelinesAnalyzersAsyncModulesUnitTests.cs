using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.AsyncModuleAnalyzer>;

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
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<CommandResult>
{
    protected override Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        {|#0:return _command.ExecuteCommandLineTool(new CommandLineToolOptions(""git""));|}
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
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        {|#0:return await _command.ExecuteCommandLineTool(new CommandLineToolOptions(""""git""""));|}
    }
}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_IEnumerable()
    {
        var expected = VerifyCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_List()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }
}