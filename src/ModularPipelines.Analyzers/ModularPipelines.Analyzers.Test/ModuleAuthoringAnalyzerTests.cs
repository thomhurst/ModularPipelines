using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAuthoringAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleAuthoringAnalyzerTests
{
    private const string Header = TestSourceConstants.StandardModuleHeaderWithExtensions;

    [TestMethod]
    public async Task Reports_Module_That_Is_Not_Registered()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }
            """;

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Registered_Module()
    {
        var source = ModuleSource(TestSourceConstants.SimpleAsyncExecuteBody);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Async_Void_Method_In_Module()
    {
        var source = ModuleSource($$"""
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

                public async void {|#0:Run|}()
                {
                    await Task.Yield();
                }
            """);

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.AsyncVoidId)
            .WithLocation(0)
            .WithArguments("Run");
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    [DataRow("_ = {|#0:Task.FromResult(\"value\").Result|};", "Result")]
    [DataRow("{|#0:Task.Delay(1).Wait()|};", "Wait")]
    [DataRow("{|#0:Task.Delay(1).GetAwaiter().GetResult()|};", "GetResult")]
    public async Task Reports_Blocking_Call_In_ExecuteAsync(
        string blockingStatement,
        string memberName)
    {
        var source = ModuleSource($$"""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                {{blockingStatement}}
                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.BlockingCallId)
            .WithLocation(0)
            .WithArguments(memberName);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:Task.Delay(1)|};
                return null;
            }
            """);

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_Through_ConfigureAwait()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:Task.Delay(1)|}.ConfigureAwait(false);
                return null;
            }
            """);

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Flowed_CancellationToken()
    {
        var source = ModuleSource(TestSourceConstants.SimpleAsyncExecuteBody);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Intentional_Derived_CancellationToken()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                await Task.Delay(1, source.Token);
                return null;
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Unrelated_CancellationToken_Overload()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Call("value");
                return null;
            }

                private static Task Call(string value) => Task.CompletedTask;

                private static Task Call(int value, CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_ThreadSleep_In_ExecuteAsync()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                {|#0:Thread.Sleep(1)|};
                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_NonPublic_Module()
    {
        var source = $$"""
            {{Header}}

            internal class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.NonPublicModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Duplicate_DependsOn()
    {
        var source = $$"""
            {{Header}}

            public abstract class DependencyModule : Module<List<string>>
            {
            }

            [DependsOn<DependencyModule>]
            [{|#0:DependsOn<DependencyModule>|}]
            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var expected = VerifyCS.Diagnostic(ModuleAuthoringAnalyzer.DuplicateDependsOnId)
            .WithLocation(0)
            .WithArguments("BuildModule", "DependencyModule");
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    private static string ModuleSource(string body)
    {
        return $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{body}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;
    }
}
