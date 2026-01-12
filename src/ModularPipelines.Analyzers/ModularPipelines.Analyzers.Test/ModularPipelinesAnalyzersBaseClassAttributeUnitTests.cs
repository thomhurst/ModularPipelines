using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.MissingDependsOnAttributeAnalyzer,
    ModularPipelines.Analyzers.MissingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersBaseClassAttributeUnitTests
{
    // These tests use a simpler header without Linq and Options - different from StandardModuleHeader
    private const string SimplerModuleHeader = @"
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;

namespace ModularPipelines.Examples.Modules;";

    private const string BaseTypeWithAttribute = $@"{SimplerModuleHeader}

public class Module1 : Module
{{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return null;
    }}
}}

public class Module2 : DependsOnModule1
{{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        var module1 = await {{|#0:GetModule<Module1>()|}};
        return null;
    }}
}}

[DependsOn<Module1>]
public abstract class DependsOnModule1 : Module
{{
}}
";

    private const string InterfaceWithAttribute = $@"{SimplerModuleHeader}

public class Module1 : Module
{{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return null;
    }}
}}

public class Module2 : Module, IDependsOnModule1
{{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        var module1 = await {{|#0:GetModule<Module1>()|}};
        return null;
    }}
}}

[DependsOn<Module1>]
public interface IDependsOnModule1
{{
}}
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