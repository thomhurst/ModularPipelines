using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ConflictingDependsOnAttributeAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersConflictingDependsOnAttributeUnitTests
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
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

[{|#0:DependsOn<Module2>|}]
public class Module1 : ModuleNew<List<string>>
{
    public override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}

[{|#1:DependsOn<Module1>|}]
public class Module2 : ModuleNew<List<string>>
{
    public override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
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
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

[{|#0:DependsOn<Module1>|}]
public class Module1 : ModuleNew<List<string>>
{
    public override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
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
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

public class Module1 : ModuleNew<List<string>>
{
    public override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}

[DependsOn<Module1>]
public class Module2 : ModuleNew<List<string>>
{
    public override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Conflicting_Dependencies()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module1", "Module2");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected1, expected2);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Dependency_Depends_On_Self()
    {
        var expected = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource2, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_No_Conflicting_Dependencies()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }
}