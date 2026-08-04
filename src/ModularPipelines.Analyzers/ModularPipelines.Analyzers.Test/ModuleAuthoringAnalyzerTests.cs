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
    public void New_Module_Authoring_Rules_Default_To_Warning()
    {
        Assert.AreEqual(
            Microsoft.CodeAnalysis.DiagnosticSeverity.Warning,
            ModuleAsyncSafetyAnalyzer.AsyncVoidRule.DefaultSeverity);
        Assert.AreEqual(
            Microsoft.CodeAnalysis.DiagnosticSeverity.Warning,
            DuplicateDependsOnAnalyzer.Rule.DefaultSeverity);
    }

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
    public async Task Reports_Module_Registered_Only_By_Unused_Private_Helper()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();

                public static void Configure()
                {
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
    public async Task Reports_Module_Registered_Only_By_Unreachable_Static_Constructor()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                static Registration() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
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
    public async Task Reports_Module_Registered_Only_Inside_Uninvoked_Lambda()
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
                    Action<IServiceCollection> register = services =>
                        services.AddSingleton<IModule, BuildModule>();
                    _ = register;
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
    public async Task Does_Not_Report_Module_Registered_By_Called_Private_Helper()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() => RegisterCore();

                private static void RegisterCore() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Reachable_Private_Constructor()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public sealed class Registration
            {
                public static void Register() => _ = Create();

                private static Registration Create() => new();

                private Registration() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_Only_By_Unconstructed_Instance_Initializer()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public sealed class Registration
            {
                private readonly object _registration =
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
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
    public async Task Does_Not_Report_Module_Registered_By_Constructed_Instance_Initializer()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public sealed class Registration
            {
                private readonly object _registration =
                    Pipeline.CreateBuilder().AddModule<BuildModule>();

                private Registration()
                {
                }

                public static void Register() => _ = new Registration();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_Only_In_Dead_Branch()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    if (false)
                    {
                        Pipeline.CreateBuilder().AddModule<BuildModule>();
                    }
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
    public async Task Reports_Module_Registered_Only_In_Dead_Switch_Case()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    switch (0)
                    {
                        case 1:
                            Pipeline.CreateBuilder().AddModule<BuildModule>();
                            break;
                    }
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
    public async Task Reports_Module_Registered_Only_In_Dead_Switch_Expression_Arm()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    _ = 0 switch
                    {
                        1 => builder.AddModule<BuildModule>(),
                        _ => builder,
                    };
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
    public async Task Reports_Module_Registered_Only_In_Shadowed_Switch_Expression_Arm()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    _ = 0 switch
                    {
                        0 => builder,
                        _ => builder.AddModule<BuildModule>(),
                    };
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
    public async Task Reports_Modules_Registered_Only_In_Dead_Pattern_Cases()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:RelationalModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#1:GuardedModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    switch (0)
                    {
                        case > 0:
                            Pipeline.CreateBuilder().AddModule<RelationalModule>();
                            break;
                    }

                    switch (0)
                    {
                        case 0 when false:
                            Pipeline.CreateBuilder().AddModule<GuardedModule>();
                            break;
                    }
                }
            }

            {{EntryPoint}}
            """;

        var relational = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("RelationalModule");
        var guarded = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(1)
            .WithArguments("GuardedModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(
            source,
            relational,
            guarded);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_Only_In_Dead_Logical_Pattern_Case()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    switch (0)
                    {
                        case > 0 and < 10:
                            Pipeline.CreateBuilder().AddModule<BuildModule>();
                            break;
                    }
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
    public async Task Does_Not_Report_Module_Registered_In_Do_While_False()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    do
                    {
                        Pipeline.CreateBuilder().AddModule<BuildModule>();
                    }
                    while (false);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Repeated_Factory_Return_Local_Still_Reports_Unregistered_Module()
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
                    builder.Services.AddSingleton<IModule>(_ =>
                    {
                        var module = new BuildModule();
                        if (flag)
                        {
                            return module;
                        }

                        return module;
                    });
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
    public async Task Does_Not_Report_Module_Registered_By_DI_Factory_Helper()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton<IModule>(_ => CreateModule());
                }

                private static BuildModule CreateModule() => new();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Delegated_DI_Factory()
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
                public static void Register(Func<IModule> factory)
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton<IModule>(_ => factory());
                }

                public static void Configure() => Register(() => new BuildModule());
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Interface_Factory_Helper()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton<IModule>(_ => CreateModule());
                }

                private static IModule CreateModule() => new BuildModule();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Source_Unavailable_DI_Factory()
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
                    builder.Services.AddSingleton(
                        typeof(IModule),
                        _ => Activator.CreateInstance(typeof(BuildModule))!);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
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
    [DataRow("private static readonly Type ImplementationType = typeof(BuildModule);")]
    [DataRow("private static Type ImplementationType => typeof(BuildModule);")]
    public async Task Tracks_Member_Backed_DI_Implementation_Type(string declaration)
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                {{declaration}}

                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton(typeof(IModule), ImplementationType);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Runtime_Computed_DI_Implementation_Type()
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
                    builder.Services.AddSingleton(
                        typeof(IModule),
                        ChooseImplementationType());
                }

                private static Type ChooseImplementationType() => typeof(BuildModule);
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
    public async Task Does_Not_Report_Module_Registered_With_Inserted_ServiceDescriptor()
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
                    builder.Services.Insert(
                        0,
                        ServiceDescriptor.Singleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Returned_ServiceDescriptor()
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
                    builder.Services.Add(CreateDescriptor());
                }

                private static ServiceDescriptor CreateDescriptor() =>
                    ServiceDescriptor.Singleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Coalesced_ServiceDescriptor()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.Add(
                        (ServiceDescriptor?)null
                        ?? ServiceDescriptor.Singleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Passed_Through_ServiceDescriptor()
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
                    builder.Services.Add(Pass(
                        ServiceDescriptor.Singleton<IModule, BuildModule>()));
                }

                private static ServiceDescriptor Pass(ServiceDescriptor descriptor) => descriptor;
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Conditional_ServiceDescriptor()
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
                public static void Register(bool flag)
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.Add(flag
                        ? ServiceDescriptor.Singleton<IModule, BuildModule>()
                        : ServiceDescriptor.Singleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Switch_ServiceDescriptor()
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
                public static void Register(bool flag)
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.Add(flag switch
                    {
                        true => ServiceDescriptor.Singleton<IModule, BuildModule>(),
                        false => ServiceDescriptor.Singleton<IModule, BuildModule>(),
                    });
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Replaced_ServiceDescriptor()
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
                    builder.Services.Replace(
                        ServiceDescriptor.Singleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_With_Service_Collection_Indexer()
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
                    builder.Services[0] =
                        ServiceDescriptor.Singleton<IModule, BuildModule>();
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Describe_Registration_Still_Reports_Unregistered_Module()
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
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.Add(ServiceDescriptor.Describe(
                        typeof(IModule),
                        typeof(BuildModule),
                        ServiceLifetime.Singleton));
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
    public async Task Does_Not_Report_Module_With_Unresolved_Descriptor_Implementation_Type()
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
                public static void Register() => Pipeline.CreateBuilder().Services.Add(
                    ServiceDescriptor.Singleton(
                        typeof(IModule),
                        ChooseImplementationType()));

                private static Type ChooseImplementationType() => typeof(BuildModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Passed_Through_Factory_Helper()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton<IModule>(
                        _ => Pass(new BuildModule()));
                }

                private static IModule Pass(IModule module) => module;
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Passed_Through_Switch_Factory_Helper()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            internal class DeployModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton<IModule>(
                        _ => Pick(flag, new BuildModule(), new DeployModule()));
                }

                private static IModule Pick(bool flag, IModule first, IModule second) =>
                    flag switch
                    {
                        true => first,
                        false => second,
                    };
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
    public async Task Does_Not_Report_Module_Registered_With_Branch_Assigned_Descriptor()
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
                public static void Register(bool flag)
                {
                    var builder = Pipeline.CreateBuilder();
                    ServiceDescriptor descriptor;
                    if (flag)
                    {
                        descriptor = ServiceDescriptor.Singleton<IModule, BuildModule>();
                    }
                    else
                    {
                        descriptor = ServiceDescriptor.Singleton<IModule, BuildModule>();
                    }

                    builder.Services.Add(descriptor);
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
    public async Task Descriptor_Collection_Spread_Does_Not_Hide_Unregistered_Module()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;

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
                    var builder = Pipeline.CreateBuilder();
                    ServiceDescriptor[] descriptors =
                    [
                        ServiceDescriptor.Singleton<IModule, RegisteredModule>(),
                    ];
                    builder.Services.TryAddEnumerable([.. descriptors]);
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
    public async Task Does_Not_Report_Modules_Registered_In_Fluent_Chain()
    {
        var source = $$"""
            {{Header}}

            public class FirstModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class SecondModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() => Pipeline.CreateBuilder()
                    .AddModule<FirstModule>()
                    .AddModule<SecondModule>();
            }

            {{EntryPoint}}
            """;

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
    public async Task Does_Not_Report_Branch_Assigned_Params_Type_Array()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            Type[] moduleTypes;
            if (DateTime.UtcNow.Ticks > 0)
            {
                moduleTypes = [typeof(BuildModule)];
            }
            else
            {
                moduleTypes = [typeof(BuildModule)];
            }

            Pipeline.CreateBuilder().AddModules(moduleTypes);
            """);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("private static readonly Type[] ModuleTypes = [typeof(RegisteredModule)];")]
    [DataRow("private static Type[] ModuleTypes => [typeof(RegisteredModule)];")]
    public async Task Tracks_Member_Backed_Params_Type_Array(string declaration)
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
                {{declaration}}

                public static void Register() =>
                    Pipeline.CreateBuilder().AddModules(ModuleTypes);
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
    public async Task Does_Not_Report_Switch_Assigned_Params_Type_Array()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            Type[] moduleTypes;
            switch (DateTime.UtcNow.Day)
            {
                case 1:
                    moduleTypes = [typeof(BuildModule)];
                    break;
                default:
                    moduleTypes = [typeof(BuildModule)];
                    break;
            }

            Pipeline.CreateBuilder().AddModules(moduleTypes);
            """);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("flag ? typeof(FirstModule) : typeof(SecondModule)")]
    [DataRow("flag switch { true => typeof(FirstModule), false => typeof(SecondModule) }")]
    public async Task Tracks_Bounded_Conditional_Params_Types(string moduleType)
    {
        var source = $$"""
            {{Header}}

            public class FirstModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class SecondModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:UnregisteredModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag) =>
                    Pipeline.CreateBuilder().AddModules({{moduleType}});
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
    public async Task Reports_NonPublic_Module_When_Params_Type_Array_Property_Cannot_Be_Resolved()
    {
        var source = $$"""
            {{Header}}

            internal class {|#0:BuildModule|} : Module<List<string>>
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

        var expected = VerifyRegistrationCS.Diagnostic(ModuleRegistrationAnalyzer.NonPublicModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Branch_Assigned_Implementation_Type()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    Type implementationType;
                    if (DateTime.UtcNow.Ticks > 0)
                    {
                        implementationType = typeof(BuildModule);
                    }
                    else
                    {
                        implementationType = typeof(BuildModule);
                    }

                    var builder = Pipeline.CreateBuilder();
                    builder.Services.AddSingleton(
                        typeof(IModule),
                        implementationType);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_When_Params_Type_Array_Is_Statically_Empty()
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
                    Pipeline.CreateBuilder().AddModules(new Type[0]);
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
    [DataRow("flag ? typeof(FirstModule).Assembly : typeof(SecondModule).Assembly")]
    [DataRow("flag switch { true => typeof(FirstModule).Assembly, false => typeof(SecondModule).Assembly }")]
    public async Task Tracks_Conditional_Assembly_Scans(string assembly)
    {
        var source = $$"""
            {{Header}}

            public class FirstModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class SecondModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag) =>
                    Pipeline.CreateBuilder().AddModulesFromAssembly({{assembly}});
            }

            {{EntryPoint}}
            """;

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
    public async Task Does_Not_Report_Module_Registered_By_Branch_Assigned_Assembly()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            System.Reflection.Assembly assembly;
            if (DateTime.UtcNow.Ticks > 0)
            {
                assembly = typeof(BuildModule).Assembly;
            }
            else
            {
                assembly = typeof(BuildModule).Assembly;
            }

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
    public async Task Does_Not_Report_Module_Registered_By_GetAssembly()
    {
        var source = ModuleSource(
            TestSourceConstants.SimpleAsyncExecuteBody,
            """
            Pipeline.CreateBuilder().AddModulesFromAssembly(
                System.Reflection.Assembly.GetAssembly(typeof(BuildModule))!);
            """);

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Infer_Scanned_Assembly_From_Type_Parameter()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
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

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
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
    public async Task Reports_Module_For_Unresolved_Assembly_Helper()
    {
        var source = $$"""
            {{Header}}
            using System.Reflection;

            internal class {|#0:BuildModule|} : Module<List<string>>
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

        var unregistered = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        var nonPublic = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.NonPublicModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(
            source,
            unregistered,
            nonPublic);
    }

    [TestMethod]
    public async Task Reports_When_Assembly_Helper_Cannot_Be_Resolved()
    {
        var source = $$"""
            {{Header}}
            using System.Reflection;

            internal class {|#0:BuildModule|} : Module<List<string>>
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

        var unregistered = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        var nonPublic = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.NonPublicModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(
            source,
            unregistered,
            nonPublic);
    }

    [TestMethod]
    public async Task Reports_Module_For_Unresolved_Generic_Assembly_Helper()
    {
        var source = $$"""
            {{Header}}

            internal class {|#0:BuildModule|} : Module<List<string>>
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

        var unregistered = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        var nonPublic = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.NonPublicModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(
            source,
            unregistered,
            nonPublic);
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
    public async Task Reports_Module_When_Generic_Helper_Cannot_Be_Resolved()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
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

        var expected = VerifyRegistrationCS.Diagnostic(
                ModuleRegistrationAnalyzer.UnregisteredModuleId)
            .WithLocation(0)
            .WithArguments("BuildModule");
        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source, expected);
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
    public async Task Reports_Async_Void_Anonymous_Function_In_Module()
    {
        var source = ModuleSource($$"""
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

                private readonly Action _work = {|#0:async () => await Task.Yield()|};
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.AsyncVoidId)
            .WithLocation(0)
            .WithArguments("anonymous function");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Void_Anonymous_Event_Handler()
    {
        var source = ModuleSource($$"""
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

                public event Action? Completed;

                public BuildModule() =>
                    Completed += async () => await Task.Yield();
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Async_Void_Callback_Nested_Inside_Event_Handler()
    {
        var source = ModuleSource($$"""
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

                public event Action? Completed;

                public BuildModule() => Completed += () =>
                {
                    Action work = {|#0:async () => await Task.Yield()|};
                    work();
                };
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.AsyncVoidId)
            .WithLocation(0)
            .WithArguments("anonymous function");
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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                {{blockingStatement}}
                return Task.FromResult<List<string>>(null!);
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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                _ = {|#0:GetValueAsync().Result|};
                return Task.FromResult<List<string>>(null!);
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:Task.Delay(1)|};
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:Task.Delay(1)|}.ConfigureAwait(false);
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:FetchAsync()|};
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await foreach (var item in {|#0:GetItemsAsync()|})
                {
                    _ = item;
                }

                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                await Task.Delay(1, source.Token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_CancellationToken_Returned_By_Source_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Delay(1, Pass(cancellationToken));
                return null!;
            }

                private static CancellationToken Pass(CancellationToken token) => token;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Dead_CancellationToken_Helper_Return()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Delay(1, Pass(cancellationToken));
                return null!;
            }

                private static CancellationToken Pass(CancellationToken token)
                {
                    if (false)
                    {
                        return CancellationToken.None;
                    }

                    return token;
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_CancellationToken_Returned_By_Reduced_Extension_Helper()
    {
        var source = $$"""
            {{Header}}

            public static class CancellationTokenExtensions
            {
                public static CancellationToken Pass(this CancellationToken token) => token;
            }

            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await Task.Delay(1, cancellationToken.Pass());
                    return null!;
                }
            }
            """;

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_CancellationToken_From_Constant_Selected_Arm()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Delay(1, true
                    ? cancellationToken
                    : CancellationToken.None);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_CancellationToken_From_Constant_Switch_Arm()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Delay(1, 0 switch
                {
                    0 => cancellationToken,
                    _ => CancellationToken.None,
                });
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Linked_CancellationToken_Array()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(
                    new[] { cancellationToken, CancellationToken.None });
                await Task.Delay(1, source.Token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Linked_CancellationToken_Collection_Expression()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = CancellationTokenSource.CreateLinkedTokenSource(
                    [cancellationToken, CancellationToken.None]);
                await Task.Delay(1, source.Token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Linked_CancellationToken_Collection_Spread()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var tokens = new[] { cancellationToken, CancellationToken.None };
                using var source = CancellationTokenSource.CreateLinkedTokenSource([.. tokens]);
                await Task.Delay(1, source.Token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Shared_Local_In_Conditional_Token_Branches()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = cancellationToken;
                await Task.Delay(1, context is not null ? token : token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Incidental_CancellationToken_Reference()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:Task.Delay(
                    1,
                    cancellationToken.CanBeCanceled
                        ? CancellationToken.None
                        : default)|};
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                using var source = new CancellationTokenSource();
                await {|#0:Task.Delay(1, source.Token)|};
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var pending = {|#0:Task.Delay(1)|};
                await pending;
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task pending;
                pending = {|#0:Task.Delay(1)|};
                await pending;
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Arbitrary_Cancellation_Carrier_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:Task.Delay(
                    1,
                    Select(cancellationToken, CancellationToken.None))|};
                return null!;
            }

                private static CancellationToken Select(
                    CancellationToken first,
                    CancellationToken second) => second;
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Cancellation_Overload_For_Task_Property_Receiver()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await GetHolder().Pending;
                return null!;
            }

                private static Holder GetHolder() => new();

                private static Holder GetHolder(CancellationToken cancellationToken) => new();

                private sealed class Holder
                {
                    public Task Pending { get; } = Task.CompletedTask;
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_For_Branch_Assigned_Stored_Task()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
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
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                token = cancellationToken;
                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Branch_Dependent_CancellationToken_Flow()
    {
        var cancellationLast = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
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

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);
        var cancellationFirst = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
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

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(cancellationLast, expected);
        await VerifyAsyncCS.VerifyAnalyzerAsync(cancellationFirst, expected);
    }

    [TestMethod]
    public async Task Reports_When_All_Branch_Tokens_Are_Unflowed()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                CancellationToken token;
                if (context is not null)
                {
                    token = default;
                }
                else
                {
                    token = CancellationToken.None;
                }

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_When_All_Branch_Tokens_Are_Flowed()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
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
                    token = cancellationToken;
                }

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_When_All_Switch_Tokens_Are_Flowed()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                switch (context is not null)
                {
                    case true:
                        token = cancellationToken;
                        break;
                    default:
                        token = cancellationToken;
                        break;
                }

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Token_Overwritten_In_Do_Loop()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                do
                {
                    token = cancellationToken;
                }
                while (DateTime.UtcNow.Ticks > 0);

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Token_When_Do_Loop_Can_Break_Before_Assignment()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                do
                {
                    if (DateTime.UtcNow.Ticks > 0)
                    {
                        break;
                    }

                    token = cancellationToken;
                }
                while (DateTime.UtcNow.Ticks > 0);

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Switch_Expression_CancellationToken()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = (context is not null) switch
                {
                    true => cancellationToken,
                    false => cancellationToken,
                };
                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Throwing_Switch_Expression_Arm()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = (context is not null) switch
                {
                    true => cancellationToken,
                    false => throw new InvalidOperationException(),
                };
                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Throwing_Conditional_Arm()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = context is not null
                    ? cancellationToken
                    : throw new InvalidOperationException();
                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Require_Token_Assignment_In_Returning_Switch_Case()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                switch (context is not null)
                {
                    case true:
                        return null!;
                    default:
                        token = cancellationToken;
                        break;
                }

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Ignores_Token_Initializer_Overwritten_In_All_Branches()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                if (context is not null)
                {
                    token = cancellationToken;
                }
                else
                {
                    token = cancellationToken;
                }

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Ignores_Values_Overwritten_Inside_All_Branches()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                CancellationToken token;
                if (context is not null)
                {
                    token = CancellationToken.None;
                    token = cancellationToken;
                }
                else
                {
                    token = CancellationToken.None;
                    token = cancellationToken;
                }

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Token_Initializer_Reaching_One_Branch()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                if (context is not null)
                {
                    token = cancellationToken;
                }

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Conditional_Unflowed_Token_After_Flowed_Initializer()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = cancellationToken;
                if (context is not null)
                {
                    token = CancellationToken.None;
                }

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Conditional_Unflowed_Token_After_Fully_Assigning_Branch()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
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
                    token = cancellationToken;
                }

                if (DateTime.UtcNow.Ticks > 0)
                {
                    token = CancellationToken.None;
                }

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Nested_Conditional_Unflowed_Token_In_Branch()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                CancellationToken token;
                if (context is not null)
                {
                    token = cancellationToken;
                    if (DateTime.UtcNow.Ticks > 0)
                    {
                        token = CancellationToken.None;
                    }
                }
                else
                {
                    token = cancellationToken;
                }

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Nested_Conditional_Unflowed_Token_In_Switch_Case()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                CancellationToken token;
                switch (context is not null)
                {
                    case true:
                        token = cancellationToken;
                        if (DateTime.UtcNow.Ticks > 0)
                        {
                            token = CancellationToken.None;
                        }

                        break;
                    default:
                        token = cancellationToken;
                        break;
                }

                await {|#0:Task.Delay(1, token)|};
                return null!;
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_CancellationToken_Inside_WhenAll()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.WhenAll({|#0:Task.Delay(1)|});
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WrapAsync({|#0:FetchAsync()|}, cancellationToken);
                return null!;
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
    public async Task Does_Not_Report_Selector_In_Awaitable_Constructor()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await new Awaitable(GetValue());
                return null!;
            }

                private static int GetValue() => 1;

                private static int GetValue(CancellationToken cancellationToken) => 1;

                private readonly struct Awaitable
                {
                    public Awaitable(int value)
                    {
                    }

                    public System.Runtime.CompilerServices.TaskAwaiter GetAwaiter() =>
                        Task.CompletedTask.GetAwaiter();
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Cancellation_Overload_In_Awaited_Condition()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (ShouldUseFirst() ? FetchAsync() : OtherAsync());
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (ShouldUseFirst() ? {|#0:FetchAsync()|} : OtherAsync());
                return null!;
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
    public async Task Does_Not_Report_Cancellation_Overload_In_Dead_Awaited_Conditional_Arm()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (true ? Task.CompletedTask : FetchAsync());
                return null!;
            }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Cancellation_Overload_In_Dead_Awaited_Switch_Arm()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (0 switch
                {
                    0 => Task.CompletedTask,
                    _ => FetchAsync(),
                });
                return null!;
            }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_In_Constant_False_Branch()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                if (false)
                {
                    Thread.Sleep(1);
                    Task.Delay(1).Wait();
                    _ = Task.FromResult(1).Result;
                    await FetchAsync();
                }

                return null!;
            }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Cancellation_Overloads_In_Awaited_Switch_Control()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await (SelectArm() switch
                {
                    0 when ShouldFetch() => FetchAsync(),
                    _ => OtherAsync(),
                });
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var tasks = new[] { Task.Delay(1, cancellationToken) };
                await tasks[GetIndex()];
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:FetchAsync()|}.ContinueWith(
                    _ => { },
                    cancellationToken);
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WrapAsync(await {|#0:FetchAsync()|});
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Call("value");
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync();
                return null!;
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
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await Api.FetchAsync();
                    return null!;
                }
            }
            """;

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Static_CancellationToken_Overload_For_Instance_Call()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await new Client().FetchAsync();
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#0:FetchAsync("value")|};
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync();
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync<string>();
                return null!;
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
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await {|#0:new Client().FetchAsync()|};
                    return null!;
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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                {|#0:Thread.Sleep(1)|};
                return Task.FromResult<List<string>>(null!);
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FetchAsync();
                return null!;

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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#2:Task.Run(Work)|};
                Func<Task> callback = OtherWork;
                await callback();
                return null!;

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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Func<Task> run = async () =>
                {
                    await {|#0:Task.Delay(1)|};
                };

                await run();
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Run(
                    async () => await {|#0:Task.Delay(1)|},
                    cancellationToken);
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Run(() => {|#0:FetchAsync()|}, cancellationToken);
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task Work() => {|#0:FetchAsync()|};

                await Task.Run(Work, cancellationToken);
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task Work() => {|#0:FetchAsync()|};

                await Work();
                return null!;
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
    public async Task Does_Not_Report_Unflowed_Token_In_Dead_Local_Function_Return()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task Work()
                {
                    if (false)
                    {
                        return FetchAsync();
                    }

                    return FetchAsync(cancellationToken);
                }

                await Work();
                return null!;
            }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Invoked_Member_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WorkAsync(cancellationToken);
                return null!;
            }

                private static async Task WorkAsync(CancellationToken cancellationToken)
                {
                    await {|#0:Task.Delay(1)|};
                    {|#1:Thread.Sleep(1)|};
                }
            """);

        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(1);
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            unflowedToken,
            blockingCall);
    }

    [TestMethod]
    public async Task Does_Not_Report_Flowed_Token_In_Invoked_Member_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await FirstAsync(cancellationToken);
                return null!;
            }

                private static Task FirstAsync(CancellationToken cancellationToken) =>
                    SecondAsync(cancellationToken);

                private static Task SecondAsync(CancellationToken cancellationToken) =>
                    Task.Delay(1, cancellationToken);
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Flowed_Second_Token_In_Member_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WorkAsync(CancellationToken.None, cancellationToken);
                return null!;
            }

                private static async Task WorkAsync(
                    CancellationToken unrelatedToken,
                    CancellationToken moduleToken)
                {
                    await Task.Delay(1, moduleToken);
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Flowed_Token_In_Local_Function()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WorkAsync(cancellationToken);
                return null!;

                static async Task WorkAsync(CancellationToken token)
                {
                    await Task.Delay(1, token);
                }
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unrelated_First_Token_In_Member_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WorkAsync(CancellationToken.None, cancellationToken);
                return null!;
            }

                private static async Task WorkAsync(
                    CancellationToken unrelatedToken,
                    CancellationToken moduleToken)
                {
                    await {|#0:Task.Delay(1, unrelatedToken)|};
                }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_When_Token_Position_Differs_Across_Helper_Calls()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WorkAsync(CancellationToken.None, cancellationToken);
                await WorkAsync(cancellationToken, CancellationToken.None);
                return null!;
            }

                private static async Task WorkAsync(
                    CancellationToken firstToken,
                    CancellationToken secondToken)
                {
                    await {|#0:Task.Delay(1, firstToken)|};
                }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Unflowed_Token_In_Task_Returning_Member_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await WorkAsync(cancellationToken);
                return null!;
            }

                private static Task WorkAsync(CancellationToken cancellationToken) =>
                    {|#0:FetchAsync()|};

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Inherited_Member_Helper()
    {
        var source = $$"""
            {{Header}}

            public abstract class BaseModule : Module<List<string>>
            {
                protected async Task WorkAsync(CancellationToken cancellationToken)
                {
                    await {|#0:Task.Delay(1)|};
                    {|#1:Thread.Sleep(1)|};
                }
            }

            public class BuildModule : BaseModule
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await WorkAsync(cancellationToken);
                    return null!;
                }
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(1);
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            unflowedToken,
            blockingCall);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Invoked_Property_Getter()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Pending;
                return null!;
            }

                private Task Pending
                {
                    get
                    {
                        {|#0:Thread.Sleep(1)|};
                        return {|#1:FetchAsync()|};
                    }
                }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(1)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            blockingCall,
            unflowedToken);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Invoked_Property_Setter()
    {
        var source = ModuleSource("""
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Value = 1;
                return Task.FromResult<List<string>>(null!);
            }

                private int Value
                {
                    set => {|#0:Thread.Sleep(value)|};
                }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Virtual_Helper_Override()
    {
        var source = $$"""
            {{Header}}

            public abstract class BaseModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await WorkAsync(cancellationToken);
                    return null!;
                }

                protected abstract Task WorkAsync(CancellationToken cancellationToken);
            }

            public class BuildModule : BaseModule
            {
                protected override async Task WorkAsync(
                    CancellationToken cancellationToken)
                {
                    await {|#0:Task.Delay(1)|};
                    {|#1:Thread.Sleep(1)|};
                }
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(0)
            .WithArguments("Delay");
        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(1);
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            unflowedToken,
            blockingCall);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Virtual_Property_Override()
    {
        var source = $$"""
            {{Header}}

            public abstract class BaseModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await Pending;
                    return null!;
                }

                protected abstract Task Pending { get; }
            }

            public class BuildModule : BaseModule
            {
                protected override Task Pending
                {
                    get
                    {
                        {|#0:Thread.Sleep(1)|};
                        return Task.CompletedTask;
                    }
                }
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Interface_Property_Implementation()
    {
        var source = $$"""
            {{Header}}

            public interface IWorker
            {
                Task Pending { get; }
            }

            public class BuildModule : Module<List<string>>, IWorker
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await ((IWorker)this).Pending;
                    return null!;
                }

                Task IWorker.Pending
                {
                    get
                    {
                        {|#0:Thread.Sleep(1)|};
                        return Task.CompletedTask;
                    }
                }
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_In_Unused_Member_Helper()
    {
        var source = ModuleSource("""
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<List<string>>(null!);
            }

                private static async Task WorkAsync(CancellationToken cancellationToken)
                {
                    await Task.Delay(1);
                    Thread.Sleep(1);
                }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_In_Dead_Member_Helper_Call()
    {
        var source = ModuleSource("""
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                if (false)
                {
                    Dangerous();
                }

                return Task.FromResult<List<string>>(null!);
            }

                private static void Dangerous() => Thread.Sleep(1);
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Source_Helper_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await RunAsync(async () =>
                {
                    {|#0:Thread.Sleep(1)|};
                    await {|#1:FetchAsync()|};
                });
                return null!;
            }

                private static Task RunAsync(Func<Task> callback) => callback();

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(1)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            blockingCall,
            unflowedToken);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Forwarded_Source_Callback()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await RunAsync(async () =>
                {
                    {|#0:Thread.Sleep(1)|};
                    await {|#1:FetchAsync()|};
                });
                return null!;
            }

                private static Task RunAsync(Func<Task> callback) =>
                    ForwardAsync(callback);

                private static Task ForwardAsync(Func<Task> callback) => callback();

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(1)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            blockingCall,
            unflowedToken);
    }

    [TestMethod]
    public async Task Reports_Unflowed_Token_In_Directly_Invoked_Delegate()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Func<Task> work = () => {|#0:FetchAsync()|};

                await work();
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Task Work() => {|#0:FetchAsync()|};
                Func<Task> callback = Work;

                await Task.Run(callback, cancellationToken);
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.WhenAll(
                    Enumerable.Range(0, 1).Select(
                        async _ => await {|#0:Task.Delay(1)|}));
                return null!;
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
            protected override Task<List<string>> ExecuteAsync(
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
                return Task.FromResult<List<string>>(null!);
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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                _ = new[] { 1 }.Any(value =>
                {
                    {|#0:Thread.Sleep(1)|};
                    return value > 0;
                });
                return Task.FromResult<List<string>>(null!);
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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                new List<int> { 1 }.ForEach(_ => {|#0:Thread.Sleep(1)|});
                return Task.FromResult<List<string>>(null!);
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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Parallel.ForEach(new[] { 1 }, _ => {|#0:Thread.Sleep(1)|});
                return Task.FromResult<List<string>>(null!);
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.CompletedTask.ContinueWith(
                    _ => {|#0:Thread.Sleep(1)|},
                    cancellationToken);
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
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
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
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
                return null!;
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
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Parallel.ForEachAsync(
                    new[] { 1 },
                    cancellationToken,
                    async (_, token) => await Task.Delay(1, token));
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Unrelated_Parallel_ForEachAsync_Callback_Token()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await {|#1:Parallel.ForEachAsync(
                    new[] { 1 },
                    CancellationToken.None,
                    async (_, token) => await {|#0:Task.Delay(1, token)|})|};
                return null!;
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
            protected override Task<List<string>> ExecuteAsync(
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

                return Task.FromResult<List<string>>(null!);
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
            protected override Task<List<string>> ExecuteAsync(
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

                return Task.FromResult<List<string>>(null!);
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
            protected override async Task<List<string>> ExecuteAsync(
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
                return null!;
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
            protected override Task<List<string>> ExecuteAsync(
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

                return Task.FromResult<List<string>>(null!);
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_Inside_Unused_Linq_Callback()
    {
        var source = ModuleSource("""
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var tasks = Enumerable.Range(0, 1).Select(
                    async _ => await Task.Delay(1));
                return Task.FromResult<List<string>>(null!);
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_Inside_Unused_Lambda()
    {
        var source = ModuleSource("""
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Func<Task> run = async () =>
                {
                    await Task.Delay(1);
                };

                return Task.FromResult<List<string>>(null!);
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Async_Safety_Inside_Unused_Local_Function()
    {
        var source = ModuleSource("""
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<List<string>>(null!);

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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<List<string>>(null!);

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
            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                return Task.FromResult<List<string>>(null!);

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
    public async Task Does_Not_Report_Branch_Assigned_Instance_Registration()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    var builder = Pipeline.CreateBuilder();
                    IModule module;
                    if (flag)
                    {
                        module = new BuildModule();
                    }
                    else
                    {
                        module = new BuildModule();
                    }

                    builder.Services.AddSingleton<IModule>(module);
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
    public async Task Reports_Module_From_Unselected_Constant_Factory_Branch()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:DeployModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() => Pipeline.CreateBuilder().Services
                    .AddSingleton<IModule>(_ => true
                        ? new BuildModule()
                        : new DeployModule());
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
    public async Task Reports_Module_From_Dead_Factory_Return()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:DeployModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() => Pipeline.CreateBuilder().Services
                    .AddSingleton<IModule>(_ =>
                    {
                        if (false)
                        {
                            return new DeployModule();
                        }

                        return new BuildModule();
                    });
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
    public async Task Reports_Module_From_Dead_Factory_Helper_Return()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:DeployModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() => Pipeline.CreateBuilder().Services
                    .AddSingleton<IModule>(_ => CreateModule());

                private static IModule CreateModule()
                {
                    if (false)
                    {
                        return new DeployModule();
                    }

                    return new BuildModule();
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
    public async Task Does_Not_Report_Module_Registered_By_Switch_Factory()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag) => Pipeline.CreateBuilder().Services
                    .AddSingleton<IModule>(_ => flag switch
                    {
                        true => new BuildModule(),
                        false => new BuildModule(),
                    });
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
    public async Task Does_Not_Report_Module_Registered_By_Startup_Method_Group()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(RegisterModules);
                }

                private static void RegisterModules(IServiceCollection services) =>
                    services.AddSingleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Local_Startup_Method_Group()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    Action<IServiceCollection> callback = RegisterModules;
                    builder.ConfigureServices(callback);
                }

                private static void RegisterModules(IServiceCollection services) =>
                    services.AddSingleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Field_Assembly()
    {
        var source = $$"""
            {{Header}}
            using System.Reflection;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Assembly Modules = typeof(BuildModule).Assembly;

                public static void Register() =>
                    Pipeline.CreateBuilder().AddModulesFromAssembly(Modules);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Property_Assembly()
    {
        var source = $$"""
            {{Header}}
            using System.Reflection;

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static Assembly Modules => typeof(BuildModule).Assembly;

                public static void Register() =>
                    Pipeline.CreateBuilder().AddModulesFromAssembly(Modules);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Direct_Delegate_Invoke()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    Action callback = RegisterModules;
                    callback();
                }

                private static void RegisterModules() =>
                    Pipeline.CreateBuilder().Services.AddSingleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Returned_Startup_Callback()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(GetRegistration());
                }

                private static Action<IServiceCollection> GetRegistration() => RegisterModules;

                private static void RegisterModules(IServiceCollection services) =>
                    services.AddSingleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Field_Startup_Method_Group()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Action<IServiceCollection> Callback = RegisterModules;

                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(Callback);
                }

                private static void RegisterModules(IServiceCollection services) =>
                    services.AddSingleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Helper_Assigned_Callback()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static Action Callback = null!;

                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    SetCallback(builder);
                    Callback();
                }

                private static void SetCallback(PipelineBuilder builder) =>
                    Callback = () => builder.AddModule<BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Static_Constructor_Field_Assignment()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Action<IServiceCollection> Callback;

                static Registration() => Callback = RegisterModules;

                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(Callback);
                }

                private static void RegisterModules(IServiceCollection services) =>
                    services.AddSingleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Static_Event_Type_Initializer()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            internal static class RegistrationCallbacks
            {
                static RegistrationCallbacks() =>
                    Pipeline.CreateBuilder().Services
                        .AddSingleton<IModule, BuildModule>();

                public static event Action Ready
                {
                    add { }
                    remove { }
                }
            }

            public static class Registration
            {
                public static void Register() =>
                    RegistrationCallbacks.Ready += static () => { };
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_When_Static_Callback_Assignment_Is_Overwritten()
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
                private static readonly Action<IServiceCollection> Callback;

                static Registration()
                {
                    Callback = RegisterModules;
                    Callback = static _ => { };
                }

                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(Callback);
                }

                private static void RegisterModules(IServiceCollection services) =>
                    services.AddSingleton<IModule, BuildModule>();
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
    public async Task Does_Not_Report_Static_Callback_Overwritten_By_Exhaustive_Branch()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Action<IServiceCollection> Callback;

                static Registration()
                {
                    Callback = static _ => { };
                    if (DateTime.UtcNow.Ticks > 0)
                    {
                        Callback = RegisterModules;
                    }
                    else
                    {
                        Callback = RegisterModules;
                    }
                }

                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(Callback);
                }

                private static void RegisterModules(IServiceCollection services) =>
                    services.AddSingleton<IModule, BuildModule>();
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Unresolved_Descriptor_In_Collection_Suppresses_Unknown_Module()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;

            public class KnownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class UnknownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.TryAddEnumerable(
                    [
                        ServiceDescriptor.Singleton<IModule, KnownModule>(),
                        ServiceDescriptor.Singleton(typeof(IModule), ChooseImplementationType()),
                    ]);
                }

                private static Type ChooseImplementationType() => typeof(UnknownModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Startup_Lambda()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(services =>
                        services.AddSingleton<IModule, BuildModule>());
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Startup_Property_Getter()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static bool IsRegistered
                {
                    get
                    {
                        Pipeline.CreateBuilder().Services
                            .AddSingleton<IModule, BuildModule>();
                        return true;
                    }
                }

                public static void Register() => _ = IsRegistered;
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_Only_In_Constant_False_For_Loop()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    for (; false;)
                    {
                        Pipeline.CreateBuilder().AddModule<BuildModule>();
                    }
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
    public async Task Does_Not_Report_Branch_Assigned_Module_Service_Type()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    Type serviceType;
                    if (flag)
                    {
                        serviceType = typeof(IModule);
                    }
                    else
                    {
                        serviceType = typeof(IModule);
                    }

                    Pipeline.CreateBuilder().Services.AddSingleton(
                        serviceType,
                        typeof(BuildModule));
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Branch_Assigned_Descriptor_Implementation_Type()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    Type implementationType;
                    if (flag)
                    {
                        implementationType = typeof(BuildModule);
                    }
                    else
                    {
                        implementationType = typeof(BuildModule);
                    }

                    Pipeline.CreateBuilder().Services.Add(
                        ServiceDescriptor.Singleton(
                            typeof(IModule),
                            implementationType));
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Conditional_Descriptor_Implementation_Type()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    Pipeline.CreateBuilder().Services.Add(
                        ServiceDescriptor.Singleton(
                            typeof(IModule),
                            flag ? typeof(BuildModule) : typeof(BuildModule)));
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Registered_By_Field_Startup_Lambda()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Action<IServiceCollection> Callback = services =>
                    services.AddSingleton<IModule, BuildModule>();

                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.ConfigureServices(Callback);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_With_Local_Unresolved_Descriptor()
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
                    var services = Pipeline.CreateBuilder().Services;
                    var descriptor = ServiceDescriptor.Singleton(
                        typeof(IModule),
                        ChooseImplementationType());
                    services.Add(descriptor);
                }

                private static Type ChooseImplementationType() => typeof(BuildModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_With_Member_Service_Descriptor()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            internal class DeployModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly ServiceDescriptor Descriptor =
                    ServiceDescriptor.Singleton<IModule, BuildModule>();

                private static ServiceDescriptor PropertyDescriptor =>
                    ServiceDescriptor.Singleton<IModule, DeployModule>();

                public static void Register()
                {
                    var services = Pipeline.CreateBuilder().Services;
                    services.Add(Descriptor);
                    services.Add(PropertyDescriptor);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Module_Instance_Stored_In_Member()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            internal class DeployModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly IModule Instance = new BuildModule();

                private static IModule Property => new DeployModule();

                public static void Register()
                {
                    var services = Pipeline.CreateBuilder().Services;
                    services.AddSingleton(Instance);
                    services.AddSingleton(Property);
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Unresolved_Descriptor_Stored_In_Member_Suppresses_Module()
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
                private static readonly ServiceDescriptor Descriptor =
                    ServiceDescriptor.Singleton(
                        typeof(IModule),
                        ChooseImplementationType());

                public static void Register() =>
                    Pipeline.CreateBuilder().Services.Add(Descriptor);

                private static Type ChooseImplementationType() => typeof(BuildModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_From_Nested_Descriptor_Member_Return()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:DeployModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static ServiceDescriptor Descriptor
                {
                    get
                    {
                        Func<ServiceDescriptor> unused = () =>
                            ServiceDescriptor.Singleton<IModule, DeployModule>();
                        return ServiceDescriptor.Singleton<IModule, BuildModule>();
                    }
                }

                public static void Register() =>
                    Pipeline.CreateBuilder().Services.Add(Descriptor);
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
    public async Task Reports_Module_When_ServiceDescriptor_Reaches_Custom_ServiceCollection_Add()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class NoOpServiceCollectionExtensions
            {
                public static void Add(
                    this IServiceCollection services,
                    ServiceDescriptor descriptor,
                    bool ignored)
                {
                }
            }

            public static class Registration
            {
                public static void Register()
                {
                    var builder = Pipeline.CreateBuilder();
                    builder.Services.Add(
                        ServiceDescriptor.Singleton<IModule, BuildModule>(),
                        ignored: true);
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
    public async Task Unresolved_Descriptor_Passed_Through_Helper_Suppresses_Module()
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
                public static void Register() =>
                    Pipeline.CreateBuilder().Services.Add(Pass(
                        ServiceDescriptor.Singleton(
                            typeof(IModule),
                            ChooseImplementationType())));

                private static ServiceDescriptor Pass(ServiceDescriptor descriptor) => descriptor;

                private static Type ChooseImplementationType() => typeof(BuildModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Unresolved_Composite_Helper_Return_Suppresses_Module()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class KnownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class UnknownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag) =>
                    Pipeline.CreateBuilder().Services.Add(Choose(
                        flag,
                        ServiceDescriptor.Singleton<IModule, KnownModule>(),
                        ServiceDescriptor.Singleton(
                            typeof(IModule),
                            ChooseImplementationType())));

                private static ServiceDescriptor Choose(
                    bool flag,
                    ServiceDescriptor known,
                    ServiceDescriptor runtimeComputed) =>
                    flag ? known : runtimeComputed;

                private static Type ChooseImplementationType() => typeof(UnknownModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Unresolved_Second_Helper_Return_Suppresses_Module()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class KnownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class UnknownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag) =>
                    Pipeline.CreateBuilder().Services.Add(Choose(flag));

                private static ServiceDescriptor Choose(bool flag)
                {
                    ServiceDescriptor result;
                    if (flag)
                    {
                        result = ServiceDescriptor.Singleton<IModule, KnownModule>();
                        return result;
                    }

                    result = ServiceDescriptor.Singleton(
                        typeof(IModule),
                        ChooseImplementationType());
                    return result;
                }

                private static Type ChooseImplementationType() => typeof(UnknownModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Switch_Passed_Through_ServiceDescriptor()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    var services = Pipeline.CreateBuilder().Services;
                    services.Add(Pick(
                        flag,
                        ServiceDescriptor.Singleton<IModule, BuildModule>(),
                        ServiceDescriptor.Singleton<IModule, BuildModule>()));
                }

                private static ServiceDescriptor Pick(
                    bool flag,
                    ServiceDescriptor first,
                    ServiceDescriptor second) => flag switch
                    {
                        true => first,
                        false => second,
                    };
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_From_Dead_Descriptor_Helper_Return()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            internal class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class {|#0:DeployModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().Services.Add(CreateDescriptor());

                private static ServiceDescriptor CreateDescriptor()
                {
                    if (false)
                    {
                        return ServiceDescriptor.Singleton<IModule, DeployModule>();
                    }

                    return ServiceDescriptor.Singleton<IModule, BuildModule>();
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
    public async Task Unresolved_Conditional_Descriptor_Suppresses_Unknown_Module()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class KnownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public class UnknownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register(bool flag)
                {
                    Pipeline.CreateBuilder().Services.Add(flag
                        ? ServiceDescriptor.Singleton<IModule, KnownModule>()
                        : ServiceDescriptor.Singleton(
                            typeof(IModule),
                            ChooseImplementationType()));
                }

                private static Type ChooseImplementationType() => typeof(UnknownModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_From_Dead_Unresolved_Conditional_Descriptor()
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
                    var descriptor = true
                        ? ServiceDescriptor.Singleton<string>("value")
                        : ServiceDescriptor.Singleton(
                            typeof(IModule),
                            ChooseImplementationType());
                    Pipeline.CreateBuilder().Services.Add(descriptor);
                }

                private static Type ChooseImplementationType() => typeof(BuildModule);
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
    public async Task Reports_Module_From_Dead_Unresolved_Switch_Descriptor()
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
                    var descriptor = 0 switch
                    {
                        0 => ServiceDescriptor.Singleton<string>("value"),
                        _ => ServiceDescriptor.Singleton(
                            typeof(IModule),
                            ChooseImplementationType()),
                    };
                    Pipeline.CreateBuilder().Services.Add(descriptor);
                }

                private static Type ChooseImplementationType() => typeof(BuildModule);
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
    public async Task Unresolved_Indexer_Descriptor_Suppresses_Unknown_Module()
    {
        var source = $$"""
            {{Header}}
            using Microsoft.Extensions.DependencyInjection;

            public class UnknownModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    var services = Pipeline.CreateBuilder().Services;
                    services[0] = ServiceDescriptor.Singleton(
                        typeof(IModule),
                        ChooseImplementationType());
                }

                private static Type ChooseImplementationType() => typeof(UnknownModule);
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_By_Indexer_In_Uninvoked_Callback()
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
                    var services = Pipeline.CreateBuilder().Services;
                    Action callback = () => services[0] =
                        ServiceDescriptor.Singleton<IModule, BuildModule>();
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
    public async Task Reports_Module_Registered_Only_In_Uninvoked_Constructor_Callback()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    Action callback = () => new Registrar();
                }

                private sealed class Registrar
                {
                    public Registrar() =>
                        Pipeline.CreateBuilder().AddModule<BuildModule>();
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
    public async Task Does_Not_Report_Module_Registered_By_Invoked_Field_Constructor_Callback()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Action Callback = () => new Registrar();

                public static void Register() => Callback();

                private sealed class Registrar
                {
                    public Registrar() =>
                        Pipeline.CreateBuilder().AddModule<BuildModule>();
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_Only_By_Getter_In_Uninvoked_Callback()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    Action callback = () => { _ = IsRegistered; };
                }

                private static bool IsRegistered
                {
                    get
                    {
                        Pipeline.CreateBuilder().AddModule<BuildModule>();
                        return true;
                    }
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
    public async Task Does_Not_Report_Module_Registered_By_Invoked_Field_Property_Callback()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Action Callback = () => _ = IsRegistered;

                public static void Register() => Callback();

                private static bool IsRegistered
                {
                    get
                    {
                        Pipeline.CreateBuilder().AddModule<BuildModule>();
                        return true;
                    }
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_Only_By_Event_In_Uninvoked_Callback()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                public static void Register()
                {
                    Action callback = () => Registered += static () => { };
                }

                private static event Action Registered
                {
                    add => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    remove { }
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
    public async Task Does_Not_Report_Module_Registered_By_Event_In_Invoked_Field_Callback()
    {
        var source = $$"""
            {{Header}}

            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            public static class Registration
            {
                private static readonly Action Callback = () =>
                    Registered += static () => { };

                public static void Register() => Callback();

                private static event Action Registered
                {
                    add => Pipeline.CreateBuilder().AddModule<BuildModule>();
                    remove { }
                }
            }

            {{EntryPoint}}
            """;

        await VerifyRegistrationCS.VerifyExecutableAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Module_Registered_By_Field_In_Uninvoked_Callback()
    {
        var source = $$"""
            {{Header}}

            public class {|#0:BuildModule|} : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            internal static class RegistrationState
            {
                public static readonly bool IsRegistered;

                static RegistrationState()
                {
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
                    IsRegistered = true;
                }
            }

            public static class Registration
            {
                public static void Register()
                {
                    Action callback = () => { _ = RegistrationState.IsRegistered; };
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
    public async Task Does_Not_Report_Statically_Null_Coalesced_CancellationToken()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Delay(
                    1,
                    (CancellationToken?)null ?? cancellationToken);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Composite_CancellationToken_Returned_By_Source_Helper()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Delay(
                    1,
                    Pick(DateTime.UtcNow.Ticks > 0, cancellationToken, cancellationToken));
                return null!;
            }

                private static CancellationToken Pick(
                    bool flag,
                    CancellationToken first,
                    CancellationToken second) => flag ? first : second;
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Statically_NonNull_Coalesced_CancellationToken()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await Task.Delay(
                    1,
                    (CancellationToken?)cancellationToken ?? CancellationToken.None);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Through_Null_Conditional_Delegate()
    {
        var source = ModuleSource("""
            private readonly Action? _callback = Work;

            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                _callback?.Invoke();
                return Task.FromResult<List<string>>(null!);
            }

            private static void Work() => {|#0:Thread.Sleep(1)|};
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Through_Constructor_Assigned_Delegate()
    {
        var source = ModuleSource("""
            private readonly Action _callback;

            public BuildModule() => _callback = Work;

            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                _callback();
                return Task.FromResult<List<string>>(null!);
            }

            private static void Work() => {|#0:Thread.Sleep(1)|};
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_In_Interface_Dispatched_Helper()
    {
        var source = $$"""
            {{Header}}

            public interface IWorker
            {
                Task WorkAsync();
            }

            public class BuildModule : Module<List<string>>, IWorker
            {
                protected override async Task<List<string>> ExecuteAsync(
                    IModuleContext context,
                    CancellationToken cancellationToken)
                {
                    await ((IWorker)this).WorkAsync();
                    return null!;
                }

                async Task IWorker.WorkAsync()
                {
                    {|#0:Thread.Sleep(1)|};
                    await {|#1:FetchAsync()|};
                }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            }

            public static class Registration
            {
                public static void Register() =>
                    Pipeline.CreateBuilder().AddModule<BuildModule>();
            }
            """;

        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(1)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            blockingCall,
            unflowedToken);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Field_Stored_Source_Callback()
    {
        var source = ModuleSource("""
            private Func<Task>? _callback;

            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                await RunAsync(async () =>
                {
                    {|#0:Thread.Sleep(1)|};
                    await {|#1:FetchAsync()|};
                });
                return null!;
            }

                private Task RunAsync(Func<Task> callback)
                {
                    _callback = callback;
                    return _callback();
                }

                private static Task FetchAsync() => Task.CompletedTask;

                private static Task FetchAsync(CancellationToken cancellationToken) =>
                    Task.CompletedTask;
            """);

        var blockingCall = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        var unflowedToken = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
            .WithLocation(1)
            .WithArguments("FetchAsync");
        await VerifyAsyncCS.VerifyAnalyzerAsync(
            source,
            blockingCall,
            unflowedToken);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Module_Field_Delegate()
    {
        var source = ModuleSource("""
            private static readonly Action Callback = Work;

            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Callback();
                return Task.FromResult<List<string>>(null!);
            }

            private static void Work() => {|#0:Thread.Sleep(1)|};
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Module_Property_Delegate()
    {
        var source = ModuleSource("""
            private static Action Callback { get; } = Work;

            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Callback();
                return Task.FromResult<List<string>>(null!);
            }

            private static void Work() => {|#0:Thread.Sleep(1)|};
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Module_Event_Accessor()
    {
        var source = ModuleSource("""
            private event Action Ready
            {
                add => {|#0:Thread.Sleep(1)|};
                remove { }
            }

            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Ready += static () => { };
                return Task.FromResult<List<string>>(null!);
            }
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Reports_Async_Safety_Inside_Module_Event_Handler()
    {
        var source = ModuleSource("""
            private event Action? Ready;

            public BuildModule() => Ready += Work;

            protected override Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                Ready?.Invoke();
                return Task.FromResult<List<string>>(null!);
            }

            private static void Work() => {|#0:Thread.Sleep(1)|};
            """);

        var expected = VerifyAsyncCS.Diagnostic(
                ModuleAsyncSafetyAnalyzer.ThreadSleepId)
            .WithLocation(0);
        await VerifyAsyncCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Does_Not_Report_Token_Assigned_In_Finally_Block()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                try
                {
                }
                finally
                {
                    token = cancellationToken;
                }

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Token_Assigned_In_Try_And_Catch()
    {
        var source = ModuleSource("""
            protected override async Task<List<string>> ExecuteAsync(
                IModuleContext context,
                CancellationToken cancellationToken)
            {
                var token = CancellationToken.None;
                try
                {
                    if (DateTime.UtcNow.Ticks == 0)
                    {
                        throw new InvalidOperationException();
                    }

                    token = cancellationToken;
                }
                catch
                {
                    token = cancellationToken;
                }

                await Task.Delay(1, token);
                return null!;
            }
            """);

        await VerifyAsyncCS.VerifyAnalyzerAsync(source);
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
    public async Task Reports_Duplicate_DependsOn_On_Abstract_Module()
    {
        var source = $$"""
            {{Header}}

            public abstract class DependencyModule : Module<List<string>>
            {
            }

            [DependsOn<DependencyModule>]
            [{|#0:DependsOn<DependencyModule>|}]
            public abstract class BaseModule : Module<List<string>>
            {
            }
            """;

        var expected = VerifyDependencyCS.Diagnostic(DuplicateDependsOnAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("BaseModule", "DependencyModule");
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
