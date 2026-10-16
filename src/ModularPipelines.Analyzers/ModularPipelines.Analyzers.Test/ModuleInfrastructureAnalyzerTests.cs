using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleInfrastructureAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleInfrastructureAnalyzerTests
{
    private const string Header = TestSourceConstants.StandardUsings + """

        using System.Diagnostics;
        using System.Net.Http;
        using P = System.Diagnostics.Process;
        using H = System.Net.Http.HttpClient;
        using static System.Diagnostics.Process;
        """;

    [TestMethod]
    public void Rules_Are_Enabled_Warnings()
    {
        foreach (var rule in new ModuleInfrastructureAnalyzer().SupportedDiagnostics)
        {
            Assert.AreEqual(DiagnosticSeverity.Warning, rule.DefaultSeverity);
            Assert.IsTrue(rule.IsEnabledByDefault);
        }
    }

    [TestMethod]
    [DataRow("_ = {|#0:Process.Start(\"tool\")|};")]
    [DataRow("_ = {|#0:global::System.Diagnostics.Process.Start(\"tool\", \"--version\")|};")]
    [DataRow("_ = {|#0:P.Start(\"tool\")|};")]
    [DataRow("_ = {|#0:Start(\"tool\")|};")]
    [DataRow("using var process = new Process(); {|#0:process.Start()|};")]
    [DataRow("_ = {|#0:new ProcessStartInfo(\"tool\")|};")]
    [DataRow("ProcessStartInfo info = {|#0:new(\"tool\")|};")]
    [DataRow("using var process = new Process(); process?{|#0:.Start()|};")]
    public async Task Reports_Direct_Process_Use(string statement)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(statement),
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.DirectProcessId).WithLocation(0));
    }

    [TestMethod]
    [DataRow("_ = {|#0:new HttpClient()|};")]
    [DataRow("_ = {|#0:new global::System.Net.Http.HttpClient(new HttpClientHandler(), true)|};")]
    [DataRow("_ = {|#0:new H()|};")]
    [DataRow("HttpClient client = {|#0:new()|};")]
    public async Task Reports_Direct_HttpClient_Construction(string statement)
    {
        await VerifyCS.VerifyAnalyzerAsync(ModuleSource(statement),
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.DirectHttpClientId).WithLocation(0));
    }

    [TestMethod]
    [DataRow("_ = {|#0:new CustomClient()|};")]
    [DataRow("CustomClient client = {|#0:new()|};")]
    [DataRow("_ = {|#0:new DerivedClient()|};")]
    public async Task Reports_HttpClient_Subclass_Construction(string statement)
    {
        var source = ModuleSource(statement) + """

            public class CustomClient : HttpClient { }
            public class DerivedClient : CustomClient { }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.DirectHttpClientId).WithLocation(0));
    }

    [TestMethod]
    public async Task Reports_Initializers_Constructors_And_Nested_Callbacks_In_Inherited_Module()
    {
        var source = $$"""
            {{Header}}
            public abstract class BaseModule<T> : Module<T> { }
            public abstract partial class BuildModule : BaseModule<int>
            {
                private readonly HttpClient client = {|#0:new()|};
                protected BuildModule() { _ = {|#1:new ProcessStartInfo()|}; }
            }
            public abstract partial class BuildModule
            {
                private void Run()
                {
                    void Local() { _ = {|#2:Process.Start("tool")|}; }
                    Action callback = () => { _ = {|#3:new HttpClient()|}; };
                    Local();
                    callback();
                }
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.DirectHttpClientId).WithLocation(0),
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.DirectProcessId).WithLocation(1),
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.DirectProcessId).WithLocation(2),
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.DirectHttpClientId).WithLocation(3));
    }

    [TestMethod]
    [DataRow("protected BuildModule({|#0:Dependency dependency|}) { }")]
    [DataRow("protected BuildModule({|#0:Dependency? dependency|}) { }")]
    [DataRow("protected BuildModule({|#0:IModule dependency|}) { }")]
    public async Task Reports_Constructor_Injection_Even_With_Declared_Dependency(string constructor)
    {
        var source = $$"""
            {{Header}}
            public abstract class Dependency : Module<int> { }
            [DependsOn<Dependency>]
            public abstract class BuildModule : Module<int>
            {
                {{constructor}}
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.InjectedModuleId).WithLocation(0));
    }

    [TestMethod]
    [DataRow("Dependency<int>", "")]
    [DataRow("T", "where T : Dependency<int>")]
    [DataRow("T", "where T : U where U : Dependency<int>")]
    public async Task Reports_Primary_Constructor_Injection_And_Generic_Module_Parameter(string parameterType, string constraints)
    {
        var source = $$"""
            {{Header}}
            public abstract class Dependency<T> : Module<T> { }
            public abstract class BuildModule<T, U>({|#0:{{parameterType}} dependency|}) : Module<int>
                {{constraints}}
            {
                public {{parameterType}} Dependency => dependency;
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleInfrastructureAnalyzer.InjectedModuleId).WithLocation(0));
    }

    [TestMethod]
    public async Task Does_Not_Report_Unrelated_Types_Or_Code_Outside_Modules()
    {
        var source = $$"""
            {{Header}}
            public abstract class Dependency : Module<int> { }
            public class CustomClient : HttpClient { }
            public class Helper(Dependency dependency)
            {
                public Dependency Dependency => dependency;
                public void Run()
                {
                    _ = Process.Start("tool");
                    _ = new ProcessStartInfo();
                    _ = new HttpClient();
                    _ = new CustomClient();
                }
            }
            public abstract class BuildModule : Module<int>
            {
                protected BuildModule(HttpClient client, CustomClient customClient, Helper service) { }
                private void AcceptModule(Dependency dependency) { }
                private void Run()
                {
                    _ = new Custom.ProcessStartInfo();
                    _ = new Custom.HttpClient();
                    Custom.Process.Start();
                }
                private class NestedHelper
                {
                    private readonly HttpClient client = new();
                }
            }
            namespace Custom
            {
                public class ProcessStartInfo { }
                public class HttpClient { }
                public static class Process { public static void Start() { } }
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Pipeline_Services_And_Dependency_Accessors()
    {
        var source = $$"""
            {{Header}}
            public abstract class Dependency : Module<int> { }
            [DependsOn<Dependency>]
            public class BuildModule : Module<int>
            {
                protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    _ = await context.Shell.RunAsync("tool", new[] { "--version" }, cancellationToken);
                    _ = await context.Network.Http.SendAsync(
                        new ModularPipelines.Options.HttpOptions(new HttpRequestMessage()), cancellationToken);
                    _ = await context.GetModule<Dependency>();
                    return 1;
                }
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    private static string ModuleSource(string statement) => $$"""
        {{Header}}
        public abstract class BuildModule : Module<int>
        {
            private void Run() { {{statement}} }
        }
        """;
}
