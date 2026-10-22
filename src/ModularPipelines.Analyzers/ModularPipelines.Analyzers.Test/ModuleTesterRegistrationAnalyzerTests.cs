using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularPipelines.Testing;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleTesterRegistrationAnalyzerTests
{
    [TestMethod]
    [DataRow("BuildModule", "object")]
    [DataRow("BuildModule, object", "object")]
    [DataRow("BuildModule, UnregisteredModule", "UnregisteredModule")]
    public async Task Does_Not_Report_Module_Executed_By_ModuleTester(string typeArguments, string resultType)
    {
        var test = new VerifyCS.Test
        {
            ReferenceAssemblies = Net.Net110,
            TestCode = $$"""
                using System.Threading;
                using System.Threading.Tasks;
                using ModularPipelines;
                using ModularPipelines.Modules;
                using ModularPipelines.Testing;

                await ModuleTester.For<{{typeArguments}}>().ExecuteAsync();

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
    [DataRow("ModularPipelines.Testing")]
    [DataRow("Other.Testing")]
    public async Task Reports_Module_Passed_To_Unrelated_For_Method(string namespaceName)
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsings}}

            {{namespaceName}}.ModuleTester.For<BuildModule>();

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            namespace {{namespaceName}}
            {
                public static class ModuleTester
                {
                    public static void For<T>() { }
                }
            }
            """;

        await VerifyCS.VerifyExecutableAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
                .WithLocation(0).WithArguments("BuildModule"));
    }
}
