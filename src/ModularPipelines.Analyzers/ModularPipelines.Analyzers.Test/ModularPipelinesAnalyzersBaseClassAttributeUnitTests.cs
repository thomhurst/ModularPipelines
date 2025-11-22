using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.MissingDependsOnAttributeAnalyzer,
    ModularPipelines.Analyzers.MissingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersBaseClassAttributeUnitTests
{
    private const string BaseTypeWithAttribute = @"
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null;
    }
}

public class Module2 : DependsOnModule1
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var module1 = await {|#0:GetModule<Module1>()|};
        return null;
    }
}

[DependsOn<Module1>]
public abstract class DependsOnModule1 : Module
{
}
";

    private const string InterfaceWithAttribute = @"
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null;
    }
}

public class Module2 : Module, IDependsOnModule1
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var module1 = await {|#0:GetModule<Module1>()|};
        return null;
    }
}

[DependsOn<Module1>]
public interface IDependsOnModule1
{
}
";

    [TestMethod]
    public async Task Attribute_On_Base_Type_No_Analyzer_Error()
    {
        await VerifyCS.VerifyAnalyzerAsync(BaseTypeWithAttribute);
    }

    [TestMethod]
    public async Task Attribute_On_Interface_Type_No_Analyzer_Error()
    {
        await VerifyCS.VerifyAnalyzerAsync(InterfaceWithAttribute);
    }
}