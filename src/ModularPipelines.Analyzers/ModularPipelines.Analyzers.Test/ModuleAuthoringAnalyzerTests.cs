using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyAsyncCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;
using VerifyDependencyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.DuplicateDependsOnAnalyzer>;
using VerifyRegistrationCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleRegistrationAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleAuthoringAnalyzerTests
{
    private const string Header = TestSourceConstants.StandardModuleHeaderWithExtensions;
    private const string EntryPoint = """
        public static class Program
        {
            public static void Main()
            {
            }
        }
        """;

    [TestMethod]
    public async Task Reports_Module_That_Is_Not_Registered()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Registered_Module()
    {
        var source = ModuleSource(TestSourceConstants.SimpleAsyncExecuteBody);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Require_Registration_In_Reusable_Library()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }
            """;

        await VerifyRegistrationCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Params_Type_Array()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            "Pipeline.CreateBuilder().AddModules(typeof(BuildModule));");

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Containing_Assembly()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            "Pipeline.CreateBuilder().AddModulesFromAssemblyContainingType<BuildModule>();");

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Direct_Assembly()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            "Pipeline.CreateBuilder().AddModulesFromAssembly(typeof(BuildModule).Assembly);");

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Local_Assembly()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            var assembly = typeof(BuildModule).Assembly;
            Pipeline.CreateBuilder().AddModulesFromAssembly(assembly);
            """);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Assigned_Local_Assembly()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            System.Reflection.Assembly assembly;
            assembly = typeof(BuildModule).Assembly;
            Pipeline.CreateBuilder().AddModulesFromAssembly(assembly);
            """);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Entry_Assembly()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            Pipeline.CreateBuilder().AddModulesFromAssembly(
                System.Reflection.Assembly.GetEntryAssembly()!);
            """);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Closed_Generic_Module_Registration()
    {
        var source = $$"""
            {{Header}}

            public class ScaleModule<T> : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<ScaleModule<string>>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task External_Assembly_Scan_Does_Not_Hide_Unregistered_Local_Module()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModulesFromAssembly(typeof(string).Assembly);
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Required_AutoRegistered_Dependency()
    {
        var source = $$"""
            {{Header}}

            public class DependencyModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            [DependsOn<DependencyModule>]
            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Required_Closed_Generic_AutoRegistered_Dependency()
    {
        var source = $$"""
            {{Header}}

            public class DependencyModule<T> : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            [DependsOn<DependencyModule<string>>]
            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
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

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.AsyncVoidId)
            .WithLocation(0)
            .WithArguments("Run");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    [DataRow("_ = {|#0:Task.FromResult(\"value\").Result|};", "Result")]
    [DataRow("{|#0:Task.Delay(1).Wait()|};", "Wait")]
    [DataRow("{|#0:Task.WaitAll(Task.CompletedTask)|};", "WaitAll")]
    [DataRow("_ = {|#0:Task.WaitAny(Task.CompletedTask)|};", "WaitAny")]
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

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.BlockingCallId)
            .WithLocation(0)
            .WithArguments(memberName);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Blocking_ValueTask_Result_In_ExecuteAsync()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                _ = {|#0:GetValueAsync().Result|};
                return Task.FromResult<List<string>?>(null);
            }

                private static ValueTask<int> GetValueAsync() => ValueTask.FromResult(1);
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.BlockingCallId)
            .WithLocation(0)
            .WithArguments("Result");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
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

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
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

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Flowed_CancellationToken()
    {
        var source = ModuleSource(TestSourceConstants.SimpleAsyncExecuteBody);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
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

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unrelated_CancellationToken()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = new CancellationTokenSource();
                await {|#0:Task.Delay(1, source.Token)|};
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_For_Stored_Task()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var pending = {|#0:Task.Delay(1)|};
                await pending;
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_For_Assigned_Stored_Task()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task pending;
                pending = {|#0:Task.Delay(1)|};
                await pending;
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Uses_Latest_Assignment_When_Tracing_CancellationToken()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                token = cancellationToken;
                await Task.Delay(1, token);
                return null;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_Inside_WhenAll()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.WhenAll({|#0:Task.Delay(1)|});
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_In_Awaited_Invocation_Argument()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WrapAsync({|#0:FetchAsync()|}, cancellationToken);
                return null;
            }

                private static Task WrapAsync(Task task, CancellationToken cancellationToken) => task;

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
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

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Inaccessible_CancellationToken_Overload()
    {
        var source = $$"""
            {{Header}}

            public static class Api
            {
                public static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            }

            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>?> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await Api.FetchAsync();
                    return null;
                }
            }
            """;

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Static_CancellationToken_Overload_For_Instance_Call()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await new Client().FetchAsync();
                return null;
            }

                private sealed class Client
                {
                    public Task FetchAsync() => Task.CompletedTask;

                    public static Task FetchAsync(CancellationToken cancellationToken) =>
                        Task.CompletedTask;
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unflowed_Token_For_Generic_Overload()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:FetchAsync("value")|};
                return null;
            }

                private static Task FetchAsync<T>(T value) => Task.CompletedTask;

                private static Task FetchAsync<T>(T value, CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_Token_For_Reduced_Extension_Overload()
    {
        var source = $$"""
            {{Header}}

            public sealed class Client
            {
            }

            public static class ClientExtensions
            {
                public static Task FetchAsync(this Client client) => Task.CompletedTask;

                public static Task FetchAsync(
                    this Client client,
                    CancellationToken cancellationToken) => Task.CompletedTask;
            }

            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>?> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await {|#0:new Client().FetchAsync()|};
                    return null;
                }
            }
            """;

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_In_Unrelated_ExecuteAsync_Overload()
    {
        var source = ModuleSource($$"""
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

                private async Task ExecuteAsync(string value)
                {
                    Thread.Sleep(1);
                    await Task.Delay(1);
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
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

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Invoked_Local_Function()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync();
                return null;

                async Task FetchAsync()
                {
                    await {|#0:Task.Delay(1)|};
                }
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Invoked_Lambda()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Func<Task> run = async () =>
                {
                    await {|#0:Task.Delay(1)|};
                };

                await run();
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_TaskRun_Lambda()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Run(
                    async () => await {|#0:Task.Delay(1)|},
                    cancellationToken);
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_Inside_Unused_Lambda()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Func<Task> run = async () =>
                {
                    await Task.Delay(1);
                };

                return Task.FromResult<List<string>?>(null);
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_Inside_Unused_Local_Function()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<List<string>?>(null);

                void Block()
                {
                    Task.Delay(1).Wait();
                }
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
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

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(ModuleRegistrationAnalyzer.NonPublicModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_NonPublic_Module_Registered_By_Instance()
    {
        var source = $$"""
            {{Header}}

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule(new BuildModule());
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_NonPublic_Module_Registered_By_Factory()
    {
        var source = $$"""
            {{Header}}

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule(_ => new BuildModule());
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_NonPublic_Module_Registered_By_Assembly()
    {
        var source = $$"""
            {{Header}}

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder()
                        .AddModulesFromAssemblyContainingType<BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
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

        var expected = VerifyDependencyCS.Diagnostic(DuplicateDependsOnAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("BuildModule", "DependencyModule");
        await VerifyDependencyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Dependency_Duplicated_From_Base_Module()
    {
        var source = $$"""
            {{Header}}

            public abstract class DependencyModule : Module<List<string>>
            {
            }

            [DependsOn<DependencyModule>]
            public abstract class BaseModule : Module<List<string>>
            {
            }

            [{|#0:DependsOn<DependencyModule>|}]
            public class BuildModule : BaseModule
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var expected = VerifyDependencyCS.Diagnostic(DuplicateDependsOnAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("BuildModule", "DependencyModule");
        await VerifyDependencyCS.VerifyAnalyzerAsync(source, expected);
    }

    private static string ModuleSource(
        string body,
        string registration = "Pipeline.CreateBuilder().AddModule<BuildModule>();")
    {
        return $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{body}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    {{registration}}
                }
            }

            {{EntryPoint}}
            """;
    }
}
