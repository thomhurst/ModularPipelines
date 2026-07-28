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

public class Module1 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null;
    }
}

public class Module2 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var module1 = await {|#0:context.GetModule<Module1>()|};
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
public class Module1 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null;
    }
}

[DependsOn<Module1>]
public class Module2 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var module1 = await context.GetModule<Module1>();
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
    public async Task Generated_Accessor_Is_Triggered()
    {
        const string source = @"
#nullable enable
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Generated;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules
{
    public class Module1 : Module<string>
    {
        protected override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>(null);
    }

    public class Module2 : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var module1 = await {|#0:context.GetModule1Module()|};
            var optionalModule1 = {|#1:context.GetModule1ModuleIfRegistered()|};
            return null;
        }
    }
}

namespace ModularPipelines.Generated
{
    [GeneratedCode(""ModularPipelines.SourceGenerator"", ""1.0.0"")]
    public static class ModuleContextExtensions
    {
        public static ModularPipelines.Examples.Modules.Module1 GetModule1Module(this IModuleContext context)
            => context.GetModule<ModularPipelines.Examples.Modules.Module1>();

        public static ModularPipelines.Examples.Modules.Module1? GetModule1ModuleIfRegistered(this IModuleContext context)
            => context.GetModuleIfRegistered<ModularPipelines.Examples.Modules.Module1>();
    }
}";
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);
        var optionalExpected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(1);

        await VerifyCS.VerifyAnalyzerAsync(source, expected, optionalExpected);
    }

    [TestMethod]
    public async Task Direct_Optional_Accessor_Is_Triggered()
    {
        var source = BadModuleSource.Replace(
            "var module1 = await {|#0:context.GetModule<Module1>()|};",
            "var module1 = {|#0:context.GetModuleIfRegistered<Module1>()|};");
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Unrelated_GetModule_Is_Ignored()
    {
        const string source = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<string>
{
    protected override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string?>(null);
}

public class ModuleLookup
{
    public T GetModule<T>() where T : new() => new T();
}

public class Module2 : Module<string>
{
    protected override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var module1 = new ModuleLookup().GetModule<Module1>();
        return Task.FromResult<string?>(null);
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task CodeFixWorks()
    {
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            BadModuleSource.ReplaceLineEndings("\n"),
            expected,
            FixedModuleSource.ReplaceLineEndings("\n"));
    }

    [TestMethod]
    public async Task OptionalCodeFixWorks()
    {
        var source = BadModuleSource
            .Replace(
                "var module1 = await {|#0:context.GetModule<Module1>()|};",
                "var module1 = {|#0:context.GetModuleIfRegistered<Module1>()|};")
            .ReplaceLineEndings("\n");
        var fixedSource = FixedModuleSource
            .Replace("[DependsOn<Module1>]", "[DependsOn<Module1>(Optional = true)]")
            .Replace(
                "var module1 = await context.GetModule<Module1>();",
                "var module1 = context.GetModuleIfRegistered<Module1>();")
            .ReplaceLineEndings("\n");
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}
