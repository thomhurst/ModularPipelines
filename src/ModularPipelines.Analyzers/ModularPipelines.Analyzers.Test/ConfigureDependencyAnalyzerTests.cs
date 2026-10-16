using Microsoft.VisualStudio.TestTools.UnitTesting;
using Missing = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.MissingDependsOnAttributeAnalyzer>;
using Cycles = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ConflictingDependsOnAttributeAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ConfigureDependencyAnalyzerTests
{
    private static string CreateSource(string configuration, string dependencyAttribute = "", string extraMembers = "", string dependencyConfiguration = "") => $$"""
        {{TestSourceConstants.StandardModuleHeader}}

        {{dependencyAttribute}}
        public class Dependency : Module<string>
        {
            {{dependencyConfiguration}}
            protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult("");
        }

        public partial class Consumer : Module<string>
        {
            {{configuration}}
            {{extraMembers}}
            protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                var dependency = context.GetModule<Dependency>();
                var optional = context.GetModuleIfRegistered<Dependency>();
                return Task.FromResult("");
            }
        }
        """;

    [TestMethod]
    [DataRow("DependsOn<Dependency>()", "GetModuleIfRegistered", "MP0022")]
    [DataRow("DependsOn(typeof(Dependency))", "GetModuleIfRegistered", "MP0022")]
    [DataRow("DependsOnOptional<Dependency>()", "GetModule", "MP0021")]
    [DataRow("DependsOnOptional(typeof(Dependency))", "GetModule", "MP0021")]
    public async Task Builder_Dependency_Only_Diagnoses_Mismatched_Accessor(string call, string accessor, string diagnosticId)
    {
        var source = CreateSource($"protected override void Configure(ModuleConfigurationBuilder module) => module.{call};")
            .Replace($"context.{accessor}<Dependency>()", $"{{|#0:context.{accessor}<Dependency>()|}}");
        await Missing.VerifyAnalyzerAsync(source,
            Missing.Diagnostic(diagnosticId).WithLocation(0).WithArguments("Dependency"));
    }

    [TestMethod]
    [DataRow("DependsOnIf<Dependency>(false)")]
    [DataRow("DependsOnIf(condition: false, moduleType: typeof(Dependency))")]
    public async Task Disabled_Builder_Dependency_Does_Not_Satisfy_Accessors(string call)
    {
        var source = CreateSource($"protected override void Configure(ModuleConfigurationBuilder module) => module.{call};")
            .Replace("context.GetModule<Dependency>()", "{|#0:context.GetModule<Dependency>()|}")
            .Replace("context.GetModuleIfRegistered<Dependency>()", "{|#1:context.GetModuleIfRegistered<Dependency>()|}");
        await Missing.VerifyAnalyzerAsync(source,
            Missing.Diagnostic("MP0001").WithLocation(0).WithArguments("Dependency"),
            Missing.Diagnostic("MP0001").WithLocation(1).WithArguments("Dependency"));
    }

    [TestMethod]
    public async Task Chained_Dependency_In_Partial_Declaration_Is_Recognized()
    {
        var source = CreateSource("") + """

            public partial class Consumer
            {
                protected override void Configure(ModuleConfigurationBuilder module)
                {
                    module.WithTags("test").DependsOn<Dependency>().DependsOnOptional<Consumer>();
                }
            }
            """;
        source = source.Replace("context.GetModuleIfRegistered<Dependency>()", "{|#0:context.GetModuleIfRegistered<Dependency>()|}");
        await Missing.VerifyAnalyzerAsync(source,
            Missing.Diagnostic("MP0022").WithLocation(0).WithArguments("Dependency"));
    }

    [TestMethod]
    [DataRow("DependsOn<Dependency>()")]
    [DataRow("DependsOn(typeof(Dependency))")]
    [DataRow("DependsOnOptional<Dependency>()")]
    [DataRow("DependsOnOptional(typeof(Dependency))")]
    [DataRow("DependsOnIf<Dependency>(false)")]
    [DataRow("DependsOnIf(condition: false, moduleType: typeof(Dependency))")]
    public async Task Builder_Dependency_Participates_In_Mixed_Cycle(string call)
    {
        var source = CreateSource(
            $"protected override void Configure(ModuleConfigurationBuilder module) => {{|#1:module.{call}|}};",
            "[{|#0:DependsOn<Consumer>|}]");
        await Cycles.VerifyAnalyzerAsync(source,
            Cycles.Diagnostic().WithLocation(0).WithArguments("Consumer", "Dependency"),
            Cycles.Diagnostic().WithLocation(1).WithArguments("Dependency", "Consumer"));
    }

    [TestMethod]
    public async Task Builder_Only_Indirect_Cycle_Is_Reported()
    {
        var source = CreateSource(
                "protected override void Configure(ModuleConfigurationBuilder module) => {|#0:module.DependsOn<Dependency>()|};",
                dependencyConfiguration: "protected override void Configure(ModuleConfigurationBuilder module) => {|#1:module.DependsOn<Third>()|};")
            + """

            public class Third : Module<string>
            {
                protected override void Configure(ModuleConfigurationBuilder module) => {|#2:module.DependsOn<Consumer>()|};
                protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult("");
            }
            """;
        await Cycles.VerifyAnalyzerAsync(source,
            Cycles.Diagnostic().WithLocation(0).WithArguments("Dependency", "Consumer"),
            Cycles.Diagnostic().WithLocation(1).WithArguments("Third", "Dependency"),
            Cycles.Diagnostic().WithLocation(2).WithArguments("Consumer", "Third"));
    }

    [TestMethod]
    [DataRow("private void Other(ModuleConfigurationBuilder module) => module.DependsOn<Dependency>();")]
    [DataRow("protected override void Configure(ModuleConfigurationBuilder module) => new ModuleConfigurationBuilder().DependsOn<Dependency>();")]
    [DataRow("protected override void Configure(ModuleConfigurationBuilder module) => module.DependsOn<Consumer>();")]
    [DataRow("protected override void Configure(ModuleConfigurationBuilder module) { Action deferred = () => module.DependsOn<Dependency>(); }")]
    [DataRow("protected override void Configure(ModuleConfigurationBuilder module) { void Deferred() => module.DependsOn<Dependency>(); }")]
    public async Task Unrelated_Dependency_Does_Not_Satisfy_Accessors(string configuration)
    {
        var source = CreateSource(configuration)
            .Replace("context.GetModule<Dependency>()", "{|#0:context.GetModule<Dependency>()|}")
            .Replace("context.GetModuleIfRegistered<Dependency>()", "{|#1:context.GetModuleIfRegistered<Dependency>()|}");
        await Missing.VerifyAnalyzerAsync(source,
            Missing.Diagnostic("MP0001").WithLocation(0).WithArguments("Dependency"),
            Missing.Diagnostic("MP0001").WithLocation(1).WithArguments("Dependency"));
    }

    [TestMethod]
    public async Task Unrelated_Method_Named_DependsOn_Does_Not_Create_Cycle()
    {
        await Cycles.VerifyAnalyzerAsync(CreateSource(
            "protected override void Configure(ModuleConfigurationBuilder module) => DependsOn<Dependency>();",
            "[DependsOn<Consumer>]",
            "private void DependsOn<T>() { }"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Inherited_Configuration_Is_Recognized(bool callsBase)
    {
        var source = CreateSource(callsBase ? "protected override void Configure(ModuleConfigurationBuilder module) => base.Configure(module);" : "")
            .Replace("Consumer : Module<string>", "Consumer : ConsumerBase")
            + """

            public abstract class ConsumerBase : Module<string>
            {
                protected override void Configure(ModuleConfigurationBuilder module) => module.DependsOn<Dependency>();
            }
            """;
        source = source.Replace("context.GetModuleIfRegistered<Dependency>()", "{|#0:context.GetModuleIfRegistered<Dependency>()|}");
        await Missing.VerifyAnalyzerAsync(source,
            Missing.Diagnostic("MP0022").WithLocation(0).WithArguments("Dependency"));
    }

    [TestMethod]
    public async Task Replaced_Base_Configuration_Does_Not_Create_Cycle()
    {
        var source = CreateSource("protected override void Configure(ModuleConfigurationBuilder module) { }", "[DependsOn<Consumer>]")
            .Replace("Consumer : Module<string>", "Consumer : ConsumerBase")
            + """

            public abstract class ConsumerBase : Module<string>
            {
                protected override void Configure(ModuleConfigurationBuilder module) => module.DependsOn<Dependency>();
            }
            """;
        await Cycles.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Inherited_Builder_Dependency_Participates_In_Cycle()
    {
        var source = CreateSource("protected override void Configure(ModuleConfigurationBuilder module) => base.Configure(module);",
                "[{|#0:DependsOn<Consumer>|}]")
            .Replace("Consumer : Module<string>", "Consumer : ConsumerBase")
            + """

            public abstract class ConsumerBase : Module<string>
            {
                protected override void Configure(ModuleConfigurationBuilder module) => {|#1:module.DependsOn<Dependency>()|};
            }
            """;
        await Cycles.VerifyAnalyzerAsync(source,
            Cycles.Diagnostic().WithLocation(0).WithArguments("Consumer", "Dependency"),
            Cycles.Diagnostic().WithLocation(1).WithArguments("Dependency", "Consumer"));
    }

    [TestMethod]
    public async Task Replaced_Base_Configuration_Does_Not_Satisfy_Accessors()
    {
        var source = CreateSource("protected override void Configure(ModuleConfigurationBuilder module) { }")
            .Replace("Consumer : Module<string>", "Consumer : ConsumerBase")
            .Replace("context.GetModule<Dependency>()", "{|#0:context.GetModule<Dependency>()|}")
            .Replace("context.GetModuleIfRegistered<Dependency>()", "{|#1:context.GetModuleIfRegistered<Dependency>()|}")
            + """

            public abstract class ConsumerBase : Module<string>
            {
                protected override void Configure(ModuleConfigurationBuilder module) => module.DependsOn<Dependency>();
            }
            """;
        await Missing.VerifyAnalyzerAsync(source,
            Missing.Diagnostic("MP0001").WithLocation(0).WithArguments("Dependency"),
            Missing.Diagnostic("MP0001").WithLocation(1).WithArguments("Dependency"));
    }

    [TestMethod]
    public async Task Builder_Self_Dependency_Is_Reported()
    {
        var source = CreateSource("protected override void Configure(ModuleConfigurationBuilder module) => {|#0:module.DependsOn<Consumer>()|};");
        await Cycles.VerifyAnalyzerAsync(source,
            Cycles.Diagnostic().WithLocation(0).WithArguments("Consumer", "Consumer"));
    }

    [TestMethod]
    public async Task Acyclic_Builder_Dependencies_Are_Accepted()
    {
        await Cycles.VerifyAnalyzerAsync(CreateSource("protected override void Configure(ModuleConfigurationBuilder module) => module.WithTags(\"test\").DependsOn<Dependency>();"));
    }
}
