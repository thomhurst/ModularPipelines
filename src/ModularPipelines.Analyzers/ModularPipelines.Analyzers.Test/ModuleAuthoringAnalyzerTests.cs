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
    public async Task Does_Not_Report_Module_Registered_Directly_As_IModule_Service()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton<IModule, BuildModule>();
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Conditional_Instance_Registration_Still_Reports_Unregistered_Module()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:DeployModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    var builder = Pipeline.CreateBuilder();
                    var module = new BuildModule();
                    builder.Services.AddSingleton<IModule>(flag ? module : module);
                }
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("DeployModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Type_Based_DI()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton(typeof(IModule), typeof(BuildModule));
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_ServiceDescriptor()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.Add(
                        ServiceDescriptor.Singleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_When_ServiceDescriptor_Is_Not_Added()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    _ = ServiceDescriptor.Singleton<IModule, BuildModule>();
                }
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Module_When_ServiceDescriptor_Reaches_Unrelated_Add_Method()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public sealed class DescriptorSink
            {
                public void Add(ServiceDescriptor descriptor)
                {
                }
            }

            public static class Registration
            {
                public static void Register()
                {
                    new DescriptorSink().Add(
                        ServiceDescriptor.Singleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Constructed_ServiceDescriptor()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.Add(new ServiceDescriptor(
                        typeof(IModule),
                        typeof(BuildModule),
                        ServiceLifetime.Singleton));
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Stored_TryAddEnumerable_Descriptor()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    var descriptor = ServiceDescriptor.Singleton<IModule, BuildModule>();
                    builder.Services.TryAddEnumerable(descriptor);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_TryAdd_Descriptor()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.TryAdd(
                        ServiceDescriptor.Singleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_All_Modules_In_TryAddEnumerable_Descriptor_Array()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class DeployModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.TryAddEnumerable(
                        new[]
                        {
                            ServiceDescriptor.Singleton<IModule, BuildModule>(),
                            ServiceDescriptor.Singleton<IModule, DeployModule>(),
                        });
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("TryAddSingleton")]
    [DataRow("TryAddScoped")]
    [DataRow("TryAddTransient")]
    public async Task Does_Not_Report_Module_Registered_With_TryAdd(string registrationMethod)
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection.Extensions;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.{{registrationMethod}}<IModule, BuildModule>();
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Similarly_Named_Method_Does_Not_Register_Module()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class FakeRegistration
            {
                public static void AddModule<TModule>()
                {
                }
            }

            public static class Registration
            {
                public static void Register() =>
                    FakeRegistration.AddModule<BuildModule>();
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
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
    public async Task Does_Not_Report_Module_Registered_With_Local_Params_Type_Array()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            Type[] moduleTypes = [typeof(BuildModule)];
            Pipeline.CreateBuilder().AddModules(moduleTypes);
            """);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_When_Params_Type_Array_Property_Cannot_Be_Resolved()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static Type[] ModuleTypes { get; } =
                    typeof(Registration).Assembly.GetTypes();

                public static void Register() =>
                    Pipeline.CreateBuilder().AddModules(ModuleTypes);
            }

            {{EntryPoint}}
            """;

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
    public async Task Does_Not_Infer_Scanned_Assembly_From_Type_Parameter()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static void Register<T>() =>
                    Pipeline.CreateBuilder().AddModulesFromAssembly(typeof(T).Assembly);

                public static void RegisterExternalAssembly() => Register<string>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Collection_Spread_Does_Not_Hide_Unregistered_Module()
    {
        var source = $$"""
            {{Header}}

            public class RegisteredModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:UnregisteredModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    Type[] modules = [typeof(RegisteredModule)];
                    Pipeline.CreateBuilder().AddModules([.. modules]);
                }
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("UnregisteredModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Repeated_Local_Module_Type_Does_Not_Suppress_Diagnostics()
    {
        var source = $$"""
            {{Header}}

            public class RegisteredModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:UnregisteredModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var moduleType = typeof(RegisteredModule);
                    Pipeline.CreateBuilder().AddModules(moduleType, moduleType);
                }
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("UnregisteredModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Library_Module_When_Entry_Assembly_Is_Scanned()
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
                    Pipeline.CreateBuilder().AddModulesFromAssembly(
                        System.Reflection.Assembly.GetEntryAssembly()!);
            }
            """;

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.NonPublicModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_For_Unresolved_Assembly_Helper()
    {
        var source = $$"""
            {{Header}}
            using System.Reflection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static Assembly GetModuleAssembly() =>
                    typeof(BuildModule).Assembly;

                public static void Register() =>
                    Pipeline.CreateBuilder()
                        .AddModulesFromAssembly(GetModuleAssembly());
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Infer_Assembly_From_Nested_TypeOf_Argument()
    {
        var source = $$"""
            {{Header}}
            using System.Reflection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static Assembly Choose(Type ignored) =>
                    typeof(BuildModule).Assembly;

                public static void Register() =>
                    Pipeline.CreateBuilder()
                        .AddModulesFromAssembly(Choose(typeof(string)));
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_For_Generic_Assembly_Helper()
    {
        var source = $$"""
            {{Header}}

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static void Register<T>() =>
                    Pipeline.CreateBuilder()
                        .AddModulesFromAssemblyContainingType<T>();

                public static void RegisterBuildModule() =>
                    Register<BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Open_Generic_Module_In_Scanned_Assembly()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:ScaleModule|}<T> : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder()
                        .AddModulesFromAssemblyContainingType<ScaleModule<string>>();
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("ScaleModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
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
    public async Task Does_Not_Report_Module_Registered_Through_Generic_Helper()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register<T>()
                    where T : class, IModule =>
                    Pipeline.CreateBuilder().AddModule<T>();

                public static void RegisterBuildModule() =>
                    Register<BuildModule>();
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
    public async Task Reports_Unflowed_CancellationToken_With_Trailing_Optional_Parameter()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:FetchAsync()|};
                return null;
            }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(
                    CancellationToken cancellationToken,
                    bool refresh = false) => Task.CompletedTask;
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_In_AwaitForeach()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await foreach (var item in {|#0:GetItemsAsync()|})
                {
                    _ = item;
                }

                return null;
            }

                private static IAsyncEnumerable<int> GetItemsAsync() => null!;

                private static IAsyncEnumerable<int> GetItemsAsync(
                    CancellationToken cancellationToken) => null!;
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("GetItemsAsync");
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
    public async Task Does_Not_Report_Linked_CancellationToken_Array()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(
                    new[] { cancellationToken, CancellationToken.None });
                await Task.Delay(1, source.Token);
                return null;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Linked_CancellationToken_Collection_Expression()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(
                    [cancellationToken, CancellationToken.None]);
                await Task.Delay(1, source.Token);
                return null;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Linked_CancellationToken_Collection_Spread()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var tokens = new[] { cancellationToken, CancellationToken.None };
                using var source = CancellationTokenSource.CreateLinkedTokenSource([.. tokens]);
                await Task.Delay(1, source.Token);
                return null;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Shared_Local_In_Conditional_Token_Branches()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = cancellationToken;
                await Task.Delay(1, context is not null ? token : token);
                return null;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Incidental_CancellationToken_Reference()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:Task.Delay(
                    1,
                    cancellationToken.CanBeCanceled
                        ? CancellationToken.None
                        : default)|};
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
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
    public async Task Reports_Unflowed_CancellationToken_For_Branch_Assigned_Stored_Task()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task pending;
                if (context is not null)
                {
                    pending = {|#0:FetchAsync()|};
                }
                else
                {
                    pending = Task.CompletedTask;
                }

                await pending;
                return null;
            }

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
    public async Task Does_Not_Guess_Branch_Dependent_CancellationToken_Flow()
    {
        var cancellationLast = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                CancellationToken token;
                if (context is not null)
                {
                    token = CancellationToken.None;
                }
                else
                {
                    token = cancellationToken;
                }

                await Task.Delay(1, token);
                return null;
            }
            """);
        var cancellationFirst = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                CancellationToken token;
                if (context is not null)
                {
                    token = cancellationToken;
                }
                else
                {
                    token = CancellationToken.None;
                }

                await Task.Delay(1, token);
                return null;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(cancellationLast);
        await VerifyAsyncCS.VerifyAnalyzerAsync(cancellationFirst);
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
    public async Task Does_Not_Report_Cancellation_Overload_In_Awaited_Condition()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (ShouldUseFirst() ? FetchAsync() : OtherAsync());
                return null;
            }

                private static bool ShouldUseFirst() => true;

                private static bool ShouldUseFirst(CancellationToken cancellationToken) => true;

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task OtherAsync() => Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Cancellation_Overload_In_Awaited_Conditional_Arm()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (ShouldUseFirst() ? {|#0:FetchAsync()|} : OtherAsync());
                return null;
            }

                private static bool ShouldUseFirst() => true;

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;

                private static Task OtherAsync() => Task.CompletedTask;
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Cancellation_Overloads_In_Awaited_Switch_Control()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (SelectArm() switch
                {
                    0 when ShouldFetch() => FetchAsync(),
                    _ => OtherAsync(),
                });
                return null;
            }

                private static int SelectArm() => 0;

                private static int SelectArm(CancellationToken cancellationToken) => 0;

                private static bool ShouldFetch() => true;

                private static bool ShouldFetch(CancellationToken cancellationToken) => true;

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task OtherAsync() => Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Cancellation_Overload_In_Awaited_Index()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var tasks = new[] { Task.Delay(1, cancellationToken) };
                await tasks[GetIndex()];
                return null;
            }

                private static int GetIndex() => 0;

                private static int GetIndex(CancellationToken token) => 0;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_In_Task_Producing_Receiver()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:FetchAsync()|}.ContinueWith(
                    _ => { },
                    cancellationToken);
                return null;
            }

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
    public async Task Reports_Unflowed_CancellationToken_Once_For_Nested_Await()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WrapAsync(await {|#0:FetchAsync()|});
                return null;
            }

                private static Task WrapAsync(string value) => Task.CompletedTask;

                private static Task<string> FetchAsync() => Task.FromResult(string.Empty);

                private static Task<string> FetchAsync(CancellationToken cancellationToken) =>
                    Task.FromResult(string.Empty);
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
    public async Task Does_Not_Report_NonAwaitable_CancellationToken_Overload()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync();
                return null;
            }

                private static Task FetchAsync() => Task.CompletedTask;

                private static void FetchAsync(CancellationToken cancellationToken)
                {
                }
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
    public async Task Does_Not_Report_Generic_Cancellation_Overload_For_Non_Generic_Call()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync();
                return null;
            }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync<T>(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Void_Event_Handler_In_Module()
    {
        var source = ModuleSource($$"""
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

                public event EventHandler? Completed;

                private async void OnCompleted(object? sender, EventArgs eventArgs)
                {
                    await Task.Yield();
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Void_Custom_Event_Handler_In_Module()
    {
        var source = ModuleSource($$"""
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

                public event Action<int>? Progress;

                public BuildModule()
                {
                    Progress += OnProgress;
                }

                private async void OnProgress(int value)
                {
                    await Task.Yield();
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Generic_Overload_With_Unsatisfied_Constraints()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync<string>();
                return null;
            }

                private static Task FetchAsync<T>()
                    where T : class =>
                    Task.CompletedTask;

                private static Task FetchAsync<T>(CancellationToken cancellationToken)
                    where T : struct =>
                    Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
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
    public async Task Reports_Async_Safety_Inside_Local_Function_Method_Groups()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#2:Task.Run(Work)|};
                Func<Task> callback = OtherWork;
                await callback();
                return null;

                async Task Work()
                {
                    await {|#0:Task.Delay(1)|};
                }

                async Task OtherWork()
                {
                    await {|#1:Task.Delay(1)|};
                }
            }
            """);

        var first = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        var second = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(1)
            .WithArguments("Delay");
        var taskRun = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(2)
            .WithArguments("Run");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, first, second, taskRun);
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
    public async Task Reports_Unflowed_Token_In_Task_Returning_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Run(() => {|#0:FetchAsync()|}, cancellationToken);
                return null;
            }

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
    public async Task Reports_Unflowed_Token_In_Method_Group_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task Work() => {|#0:FetchAsync()|};

                await Task.Run(Work, cancellationToken);
                return null;
            }

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
    public async Task Reports_Unflowed_Token_In_Directly_Invoked_Local_Function()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task Work() => {|#0:FetchAsync()|};

                await Work();
                return null;
            }

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
    public async Task Reports_Unflowed_Token_In_Directly_Invoked_Delegate()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Func<Task> work = () => {|#0:FetchAsync()|};

                await work();
                return null;
            }

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
    public async Task Reports_Unflowed_Token_In_Delegate_Local_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task Work() => {|#0:FetchAsync()|};
                Func<Task> callback = Work;

                await Task.Run(callback, cancellationToken);
                return null;
            }

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
    public async Task Reports_Async_Safety_Inside_Awaited_TaskJoin_Linq_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.WhenAll(
                    Enumerable.Range(0, 1).Select(
                        async _ => await {|#0:Task.Delay(1)|}));
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    [DataRow("ToList()")]
    [DataRow("ToArray()")]
    [DataRow("Count()")]
    [DataRow("Any()")]
    [DataRow("First()")]
    [DataRow("Single()")]
    [DataRow("ToHashSet()")]
    [DataRow("Aggregate(0, (total, value) => total + value)")]
    public async Task Reports_Async_Safety_Inside_Eager_Linq_Callback(
        string terminalInvocation)
    {
        var source = ModuleSource($$"""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                _ = Enumerable.Range(0, 1)
                    .Select(_ =>
                    {
                        {|#0:Thread.Sleep(1)|};
                        return 1;
                    })
                    .{{terminalInvocation}};
                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Direct_Eager_Linq_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                _ = new[] { 1 }.Any(value =>
                {
                    {|#0:Thread.Sleep(1)|};
                    return value > 0;
                });
                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_List_ForEach_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                new List<int> { 1 }.ForEach(_ => {|#0:Thread.Sleep(1)|});
                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Parallel_ForEach_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Parallel.ForEach(new[] { 1 }, _ => {|#0:Thread.Sleep(1)|});
                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Task_Continuation()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.CompletedTask.ContinueWith(
                    _ => {|#0:Thread.Sleep(1)|},
                    cancellationToken);
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Generic_Task_Factory_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var factory = new TaskFactory<int>();
                await factory.StartNew(
                    () =>
                    {
                        {|#0:Thread.Sleep(1)|};
                        return 0;
                    },
                    cancellationToken);
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Parallel_ForEachAsync_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Parallel.ForEachAsync(
                    new[] { 1 },
                    cancellationToken,
                    async (_, _) =>
                    {
                        {|#0:Thread.Sleep(1)|};
                        await Task.Yield();
                    });
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Derived_Parallel_ForEachAsync_Callback_Token()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Parallel.ForEachAsync(
                    new[] { 1 },
                    cancellationToken,
                    async (_, token) => await Task.Delay(1, token));
                return null;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unrelated_Parallel_ForEachAsync_Callback_Token()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#1:Parallel.ForEachAsync(
                    new[] { 1 },
                    CancellationToken.None,
                    async (_, token) => await {|#0:Task.Delay(1, token)|})|};
                return null;
            }
            """);

        var delayDiagnostic = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        var forEachDiagnostic = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(1)
            .WithArguments("ForEachAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            delayDiagnostic,
            forEachDiagnostic);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Foreach_Linq_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                foreach (var value in Enumerable.Range(0, 1).Select(_ =>
                         {
                             {|#0:Thread.Sleep(1)|};
                             return 1;
                         }))
                {
                }

                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Stored_Foreach_Linq_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var values = Enumerable.Range(0, 1).Select(_ =>
                {
                    {|#0:Thread.Sleep(1)|};
                    return 1;
                });

                foreach (var value in values)
                {
                }

                return Task.FromResult<List<string>?>(null);
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Stored_Linq_Callback_Consumed_In_Awaited_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var values = Enumerable.Range(0, 1).Select(_ =>
                {
                    {|#0:Thread.Sleep(1)|};
                    return 1;
                });

                await Task.Run(() =>
                {
                    foreach (var value in values)
                    {
                    }
                }, cancellationToken);
                return null;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Overwritten_Deferred_Linq_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var values = Enumerable.Range(0, 1).Select(_ =>
                {
                    Thread.Sleep(1);
                    return 1;
                });
                values = Enumerable.Empty<int>();

                foreach (var value in values)
                {
                }

                return Task.FromResult<List<string>?>(null);
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_Inside_Unused_Linq_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var tasks = Enumerable.Range(0, 1).Select(
                    async _ => await Task.Delay(1));
                return Task.FromResult<List<string>?>(null);
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
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
    public async Task Does_Not_Report_Async_Safety_Inside_Unreachable_Recursive_Local_Function()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<List<string>?>(null);

                void Block()
                {
                    Thread.Sleep(1);
                    Block();
                }
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_Inside_Transitively_Unreachable_Local_Function()
    {
        var source = ModuleSource("""
            protected override Task<List<string>?> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<List<string>?>(null);

                void First() => Second();

                void Second()
                {
                    Thread.Sleep(1);
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
    public async Task Does_Not_Report_Instance_Registered_Through_Interface()
    {
        var source = $$"""
            {{Header}}

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    IModule module = new BuildModule();
                    Pipeline.CreateBuilder().AddModule(module);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Dependency_Of_Instance_Registered_Through_Interface()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:DependencyModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            [DependsOn<DependencyModule>]
            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    IModule module = new BuildModule();
                    Pipeline.CreateBuilder().AddModule(module);
                }
            }

            {{EntryPoint}}
            """;

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("DependencyModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
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
    public async Task Reports_Optional_Duplicate_Instead_Of_Required_Dependency()
    {
        var source = $$"""
            {{Header}}

            public abstract class DependencyModule : Module<List<string>>
            {
            }

            [{|#0:DependsOn<DependencyModule>(Optional = true)|}]
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

    [TestMethod]
    public async Task Does_Not_Report_Required_Override_Of_Optional_Base_Dependency()
    {
        var source = $$"""
            {{Header}}

            public abstract class DependencyModule : Module<List<string>>
            {
            }

            [DependsOn<DependencyModule>(Optional = true)]
            public abstract class BaseModule : Module<List<string>>
            {
            }

            [DependsOn<DependencyModule>]
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

        await VerifyDependencyCS.VerifyAnalyzerAsync(source);
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
