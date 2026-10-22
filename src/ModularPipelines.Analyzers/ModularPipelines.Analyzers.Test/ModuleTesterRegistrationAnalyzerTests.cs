using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularPipelines.Testing;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleTesterRegistrationAnalyzerTests
{
    [TestMethod]
    [DataRow("BuildModule", "object", false)]
    [DataRow("BuildModule, object", "object", false)]
    [DataRow("BuildModule, UnregisteredModule", "UnregisteredModule", false)]
    [DataRow("BuildModule", "object", true)]
    [DataRow("BuildModule, object", "object", true)]
    [DataRow("BuildModule, UnregisteredModule", "UnregisteredModule", true)]
    public async Task Does_Not_Report_Module_Executed_By_ModuleTester(string typeArguments, string resultType, bool storeBuilder)
    {
        var execution = storeBuilder
            ? $"var builder = ModuleTester.For<{typeArguments}>().ConfigurePipeline(_ => {{ }}); await builder.ExecuteAsync();"
            : $"await ModuleTester.For<{typeArguments}>().ExecuteAsync();";
        var test = new VerifyCS.Test
        {
            ReferenceAssemblies = Net.Net110,
            TestCode = $$"""
                using System.Threading;
                using System.Threading.Tasks;
                using ModularPipelines;
                using ModularPipelines.Modules;
                using ModularPipelines.Testing;

                {{execution}}

                public class BuildModule : Module<{{resultType}}>
                {
                    protected override Task<{{resultType}}> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                        => Task.FromResult(new {{resultType}}());
                }

                public class {|#0:UnregisteredModule|} : Module<object>
                {
                    protected override Task<object> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                        => Task.FromResult(new object());
                }
                """,
            TestState =
            {
                OutputKind = OutputKind.ConsoleApplication,
                AdditionalReferences =
                {
                    typeof(IModuleContext).Assembly.Location,
                    typeof(ModuleTester).Assembly.Location,
                },
            },
        };
        test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0).WithArguments("UnregisteredModule"));

        await test.RunAsync(CancellationToken.None);
    }

    [TestMethod]
    [DataRow("BuildModule", "", false)]
    [DataRow("BuildModule, object", "", false)]
    [DataRow("BuildModule", ".ConfigurePipeline(_ => { })", false)]
    [DataRow("BuildModule, object", ".ConfigurePipeline(_ => { })", false)]
    [DataRow("BuildModule", "", true)]
    [DataRow("BuildModule, object", "", true)]
    public async Task Reports_Module_When_Builder_Is_Not_Executed(string typeArguments, string configuration, bool unreachableExecution)
    {
        var unusedMethod = unreachableExecution
            ? $"static async Task Unused() {{ await ModuleTester.For<{typeArguments}>().ExecuteAsync(); }}"
            : "";
        var test = new VerifyCS.Test
        {
            ReferenceAssemblies = Net.Net110,
            TestCode = $$"""
                using System.Threading;
                using System.Threading.Tasks;
                using ModularPipelines;
                using ModularPipelines.Modules;
                using ModularPipelines.Testing;

                var configured = ModuleTester.For<{{typeArguments}}>(){{configuration}};
                {{unusedMethod}}

                public class {|#0:BuildModule|} : Module<object>
                {
                    protected override Task<object> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                        => Task.FromResult(new object());
                }
                """,
            TestState =
            {
                OutputKind = OutputKind.ConsoleApplication,
                AdditionalReferences =
                {
                    typeof(IModuleContext).Assembly.Location,
                    typeof(ModuleTester).Assembly.Location,
                },
            },
        };
        test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0).WithArguments("BuildModule"));

        await test.RunAsync(CancellationToken.None);
    }

    [TestMethod]
    [DataRow("ModularPipelines.Testing", false)]
    [DataRow("Other.Testing", false)]
    [DataRow("ModularPipelines.Testing", true)]
    [DataRow("Other.Testing", true)]
    public async Task Reports_Module_Passed_To_Unrelated_For_Method(string namespaceName, bool executeBuilder)
    {
        var execution = executeBuilder ? ".ExecuteAsync()" : "";
        var source = $$"""
            {{TestSourceConstants.StandardUsings}}

            {{namespaceName}}.ModuleTester.For<BuildModule>(){{execution}};

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            namespace {{namespaceName}}
            {
                public static class ModuleTester
                {
                    public static ModuleTestBuilder<T> For<T>() => new();
                }

                public class ModuleTestBuilder<TModule>
                {
                    public Task ExecuteAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
                }
            }
            """;

        await VerifyCS.VerifyExecutableAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
                .WithLocation(0).WithArguments("BuildModule"));
    }
}
