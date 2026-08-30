using ModularPipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.MissingDependsOnAttributeAnalyzer,
    ModularPipelines.Analyzers.MissingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersUnitTests
{
    private const string GeneratedAccessorSource = @"
#nullable enable
using System.CodeDom.Compiler;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines;

namespace ModularPipelines.Examples.Modules
{
    public class Module1 : Module<string>
    {
        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult<string>(null!);
    }

    public class Module2 : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var module1 = await {|#0:context.GetModule1Module()|};
            var optionalModule1 = {|#1:context.GetModule1ModuleIfRegistered()|};
            return null!;
        }
    }
}

namespace ModularPipelines.Generated
{
    [GeneratedCode(""ModularPipelines.SourceGenerator"", ""1.0.0"")]
    public static class AnalyzerTestsModuleContextExtensions
    {
        public static ModularPipelines.Examples.Modules.Module1 GetModule1Module(this IModuleContext context) => context.GetModule<ModularPipelines.Examples.Modules.Module1>();
        public static ModularPipelines.Examples.Modules.Module1? GetModule1ModuleIfRegistered(this IModuleContext context) => context.GetModuleIfRegistered<ModularPipelines.Examples.Modules.Module1>();
    }
}";

    private const string BadModuleSource = @"
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null!;
    }
}

public class Module2 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var module1 = await {|#0:context.GetModule<Module1>()|};
        return null!;
    }
}";

    private const string FixedModuleSource = @"
#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return null!;
    }
}

[DependsOn<Module1>]
public class Module2 : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var module1 = await context.GetModule<Module1>();
        return null!;
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
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);
        var optionalExpected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(1);

        await VerifyCS.VerifyAnalyzerAsync(GeneratedAccessorSource, expected, optionalExpected);
    }

    [TestMethod]
    public async Task Generated_Optional_Accessor_Code_Fix_Is_Optional()
    {
        var source = GeneratedAccessorSource
            .Replace(
                "var module1 = await {|#0:context.GetModule1Module()|};",
                "var module1 = context;")
            .Replace(
                "{|#1:context.GetModule1ModuleIfRegistered()|}",
                "{|#0:context.GetModule1ModuleIfRegistered()|}")
            .ReplaceLineEndings("\n");
        var fixedSource = source
            .Replace(
                "public class Module2 : Module<string>",
                "[DependsOn<Module1>(Optional = true)]\n    public class Module2 : Module<string>")
            .Replace(
                "{|#0:context.GetModule1ModuleIfRegistered()|}",
                "context.GetModule1ModuleIfRegistered()");
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId).WithArguments("Module1").WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
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
using ModularPipelines;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<string>
{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string>(null!);
}

public class ModuleLookup
{
    public T GetModule<T>() where T : new() => new T();
}

public class Module2 : Module<string>
{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var module1 = new ModuleLookup().GetModule<Module1>();
        return Task.FromResult<string>(null!);
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task GetModule_Calls_In_Non_Module_Types_Are_Ignored()
    {
        const string source = """
            #nullable enable
            using System.Threading.Tasks;
            using ModularPipelines;

            namespace Example;

            public class Dependency : Module<string>
            {
                protected override Task<string> ExecuteAsync(IModuleContext context, System.Threading.CancellationToken cancellationToken)
                    => Task.FromResult<string>(null!);
            }

            public class Helper
            {
                public async Task UseAsync(IModuleContext context)
                {
                    _ = await context.GetModule<Dependency>();
                }
            }

            public class Container
            {
                private class NestedHelper
                {
                    public async Task UseAsync(IModuleContext context)
                    {
                        _ = await context.GetModule<Dependency>();
                    }
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Nested_Helper_Code_Fix_Adds_Attribute_To_Enclosing_Module()
    {
        var source = """
            #nullable enable
            using System.Threading;
            using System.Threading.Tasks;
            using ModularPipelines;

            namespace Example;

            public class Dependency : Module<string>
            {
                protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    => Task.FromResult<string>(null!);
            }

            public class Consumer : Module<string>
            {
                private class Helper
                {
                    public async Task UseAsync(IModuleContext context)
                    {
                        _ = await {|#0:context.GetModule<Dependency>()|};
                    }
                }

                protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    => Task.FromResult<string>(null!);
            }
            """.ReplaceLineEndings("\n");
        var fixedSource = """
            #nullable enable
            using System.Threading;
            using System.Threading.Tasks;
            using ModularPipelines;

            namespace Example;

            public class Dependency : Module<string>
            {
                protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    => Task.FromResult<string>(null!);
            }

            [DependsOn<Dependency>]
            public class Consumer : Module<string>
            {
                private class Helper
                {
                    public async Task UseAsync(IModuleContext context)
                    {
                        _ = await context.GetModule<Dependency>();
                    }
                }

                protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    => Task.FromResult<string>(null!);
            }
            """.ReplaceLineEndings("\n");
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithArguments("Dependency")
            .WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
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
    public async Task CodeFix_Preserves_Unrelated_Formatting()
    {
        var source = """
            #nullable enable
            using System.Threading;
            using System.Threading.Tasks;
            using ModularPipelines;

            namespace Example;

            public class Dependency : Module<string>
            {
                protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    => Task.FromResult<string>(null!);
            }


            // Keep this comment and the unusual token spacing below.
            public   class Consumer : Module<string>
            {
                private const string Alignment = "keep";       // aligned comment

                protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    var dependency = await {|#0:context.GetModule<Dependency>()|}; // trailing comment
                    return Alignment;
                }
            }
            """.ReplaceLineEndings("\n");
        var fixedSource = """
            #nullable enable
            using System.Threading;
            using System.Threading.Tasks;
            using ModularPipelines;

            namespace Example;

            public class Dependency : Module<string>
            {
                protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    => Task.FromResult<string>(null!);
            }


            // Keep this comment and the unusual token spacing below.
            [DependsOn<Dependency>]
            public   class Consumer : Module<string>
            {
                private const string Alignment = "keep";       // aligned comment

                protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    var dependency = await context.GetModule<Dependency>(); // trailing comment
                    return Alignment;
                }
            }
            """.ReplaceLineEndings("\n");
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithArguments("Dependency")
            .WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Indents_Attribute_Appended_To_Existing_List()
    {
        var source = """
            #nullable enable
            using System.Threading;
            using System.Threading.Tasks;
            using ModularPipelines;

            namespace Example
            {
                public class Dependency : Module<string>
                {
                    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                        => Task.FromResult<string>(null!);
                }

                [System.Obsolete]
                public class Consumer : Module<string>
                {
                    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    {
                        var dependency = await {|#0:context.GetModule<Dependency>()|};
                        return null!;
                    }
                }
            }
            """.ReplaceLineEndings("\n");
        var fixedSource = """
            #nullable enable
            using System.Threading;
            using System.Threading.Tasks;
            using ModularPipelines;

            namespace Example
            {
                public class Dependency : Module<string>
                {
                    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                        => Task.FromResult<string>(null!);
                }

                [System.Obsolete]
                [DependsOn<Dependency>]
                public class Consumer : Module<string>
                {
                    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                    {
                        var dependency = await context.GetModule<Dependency>();
                        return null!;
                    }
                }
            }
            """.ReplaceLineEndings("\n");
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithArguments("Dependency")
            .WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [TestMethod]
    public async Task CodeFix_Separates_Inserted_Using_When_Source_Has_No_Usings()
    {
        var source = """
            #nullable enable
            namespace Example;

            public class Dependency : ModularPipelines.Module<string>
            {
                protected override System.Threading.Tasks.Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, System.Threading.CancellationToken cancellationToken)
                    => System.Threading.Tasks.Task.FromResult<string>(null!);
            }

            public class Consumer : ModularPipelines.Module<string>
            {
                protected override async System.Threading.Tasks.Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, System.Threading.CancellationToken cancellationToken)
                {
                    var dependency = await {|#0:context.GetModule<Dependency>()|};
                    return null!;
                }
            }
            """.ReplaceLineEndings("\n");
        var fixedSource = """
            using ModularPipelines;
            #nullable enable
            namespace Example;

            public class Dependency : ModularPipelines.Module<string>
            {
                protected override System.Threading.Tasks.Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, System.Threading.CancellationToken cancellationToken)
                    => System.Threading.Tasks.Task.FromResult<string>(null!);
            }

            [DependsOn<Dependency>]
            public class Consumer : ModularPipelines.Module<string>
            {
                protected override async System.Threading.Tasks.Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, System.Threading.CancellationToken cancellationToken)
                {
                    var dependency = await context.GetModule<Dependency>();
                    return null!;
                }
            }
            """.ReplaceLineEndings("\n");
        var expected = VerifyCS.Diagnostic(MissingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithArguments("Dependency")
            .WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
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
