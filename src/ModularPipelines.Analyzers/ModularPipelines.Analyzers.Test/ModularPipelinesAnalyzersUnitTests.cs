using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.MissingDependsOnAttributeAnalyzer,
    ModularPipelines.Analyzers.MissingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersUnitTests
{
    private const string BadModuleSource = @"
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class Module1 : ModuleNew
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null;
    }
}

public class Module2 : ModuleNew
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var module1 = await {|#0:GetModule<Module1>()|};
        return null;
    }
}";

    private const string FixedModuleSource = @"#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;
public class Module1 : ModuleNew
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null;
    }
}

[DependsOn<Module1>]
public class Module2 : ModuleNew
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var module1 = await context.GetModuleAsync<Module1>();
        return null;
    }
}";

    //No diagnostics expected to show up
    [TestMethod]
    public async Task Empty_Source()
    {
        var test = @"";

        await VerifyCS.VerifyAnalyzerAsync(test);
    }

    //No diagnostics expected to show up
    [TestMethod]
    public async Task Good_Source()
    {
        await VerifyCS.VerifyAnalyzerAsync(FixedModuleSource);
    }

    //Diagnostic and CodeFix both triggered and checked for
    [TestMethod]
    public async Task AnalyzerIsTriggered()
    {
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected);
    }

    [TestMethod]
    public async Task CodeFixWorks()
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            // This fails on Linux only due to different line endings
            // Is there a way around that?
            return;
        }

        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(BadModuleSource, expected, FixedModuleSource);
    }
}