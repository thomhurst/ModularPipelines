using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.MissingDependsOnAttributeAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class DependencyAccessorAnalyzerTests
{
    [TestMethod]
    [DataRow("[DependsOn<Dependency>(Optional = true)]", "", "GetModule", "MP0021")]
    [DataRow("[DependsOn(typeof(Dependency), Optional = true)]", "", "GetModule", "MP0021")]
    [DataRow("[DependsOn<Dependency>]", "", "GetModuleIfRegistered", "MP0022")]
    [DataRow("[DependsOn(typeof(Dependency))]", "", "GetModuleIfRegistered", "MP0022")]
    [DataRow("", "module.DependsOnOptional<Dependency>();", "GetModule", "MP0021")]
    [DataRow("", "module.DependsOnOptional(typeof(Dependency));", "GetModule", "MP0021")]
    [DataRow("", "module.DependsOn<Dependency>();", "GetModuleIfRegistered", "MP0022")]
    [DataRow("", "module.DependsOn(typeof(Dependency));", "GetModuleIfRegistered", "MP0022")]
    [DataRow("[DependsOn<Dependency>(Optional = true)]", "module.DependsOn<Dependency>();", "GetModuleIfRegistered", "MP0022")]
    [DataRow("[DependsOn<Dependency>(Optional = true), DependsOn<Dependency>]", "", "GetModuleIfRegistered", "MP0022")]
    [DataRow("", "module.DependsOnOptional<Dependency>().DependsOn<Dependency>();", "GetModuleIfRegistered", "MP0022")]
    [DataRow("", "var alias = module; alias.DependsOnOptional<Dependency>();", "GetModule", "MP0021")]
    [DataRow("", "module.DependsOnIf<Dependency>(true);", "GetModuleIfRegistered", "MP0022")]
    public async Task Reports_Mismatched_Accessor(string attributes, string configure, string accessor, string diagnosticId)
    {
        var source = Source(attributes, configure, $"{{|#0:context.{accessor}<Dependency>()|}}");

        await VerifyCS.VerifyAnalyzerAsync(source, new DiagnosticResult(diagnosticId,
                diagnosticId == "MP0021" ? DiagnosticSeverity.Warning : DiagnosticSeverity.Info)
            .WithLocation(0).WithArguments("Dependency"));
    }

    [TestMethod]
    [DataRow("[DependsOn<Dependency>(Optional = true)]", "", "GetModuleIfRegistered")]
    [DataRow("[DependsOn<Dependency>]", "", "GetModule")]
    [DataRow("", "module.DependsOnOptional<Dependency>();", "GetModuleIfRegistered")]
    [DataRow("", "module.DependsOn<Dependency>();", "GetModule")]
    [DataRow("[DependsOn<Dependency>(Optional = true)]", "module.DependsOn<Dependency>();", "GetModule")]
    [DataRow("[DependsOn<Dependency>]", "module.DependsOnOptional<Dependency>();", "GetModule")]
    [DataRow("", "module.DependsOnOptional<Dependency>().DependsOn<Dependency>();", "GetModule")]
    public async Task Accepts_Matched_Accessor(string attributes, string configure, string accessor)
        => await VerifyCS.VerifyAnalyzerAsync(Source(attributes, configure, $"context.{accessor}<Dependency>()"));

    [TestMethod]
    [DataRow("true", "GetModule", "MP0021")]
    [DataRow("false", "GetModuleIfRegistered", "MP0022")]
    public async Task Recognizes_Inherited_Attribute(string optional, string accessor, string diagnosticId)
    {
        var source = Source("", "", $"{{|#0:context.{accessor}<Dependency>()|}}")
            .Replace("public class Consumer : Module<string>", "public class Consumer : BaseModule")
            + $$"""

              [DependsOn<Dependency>(Optional = {{optional}})]
              public abstract class BaseModule : Module<string> { }
              """;

        await VerifyCS.VerifyAnalyzerAsync(source, new DiagnosticResult(diagnosticId,
                diagnosticId == "MP0021" ? DiagnosticSeverity.Warning : DiagnosticSeverity.Info)
            .WithLocation(0).WithArguments("Dependency"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Effective_Base_Configure(bool callsBase)
    {
        var source = Source("[DependsOn<Dependency>(Optional = true)]", callsBase ? "base.Configure(module);" : "",
                callsBase ? "context.GetModule<Dependency>()" : "context.GetModuleIfRegistered<Dependency>()")
            .Replace("public class Consumer : Module<string>", "public class Consumer : BaseModule")
            + """

              public abstract class BaseModule : Module<string>
              {
                  protected override void Configure(ModuleConfigurationBuilder module) => module.DependsOn<Dependency>();
              }
              """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Ignores_Unrelated_Same_Named_Methods()
    {
        var source = Source("[DependsOn<Dependency>(Optional = true)]", "new OtherBuilder().DependsOn<Dependency>();",
            "new Lookup().GetModule<Dependency>()") + """

            public class Lookup { public T GetModule<T>() where T : new() => new T(); }
            public class OtherBuilder { public void DependsOn<T>() { } }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("module.DependsOnIf<Dependency>(DateTime.Now.Ticks > 0);")]
    [DataRow("if (DateTime.Now.Ticks > 0) module.DependsOn<Dependency>();")]
    [DataRow("if (DateTime.Now.Ticks > 0) return; module.DependsOn<Dependency>();")]
    public async Task Does_Not_Presume_Conditional_Required_Declaration(string configure)
        => await VerifyCS.VerifyAnalyzerAsync(Source("[DependsOn<Dependency>(Optional = true)]", configure,
            "context.GetModuleIfRegistered<Dependency>()"));

    [TestMethod]
    public async Task Does_Not_Use_Unrelated_Builder_Declaration()
        => await VerifyCS.VerifyAnalyzerAsync(Source("[DependsOn<Dependency>(Optional = true)]",
            "new ModuleConfigurationBuilder().DependsOn<Dependency>();", "context.GetModuleIfRegistered<Dependency>()"));

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Inherited_Generic_Configure(bool optional)
    {
        var source = Source("", "base.Configure(module);",
                optional ? "context.GetModuleIfRegistered<Dependency>()" : "context.GetModule<Dependency>()")
            .Replace("public class Consumer : Module<string>", "public class Consumer : BaseModule<Dependency>")
            + $$"""

              public abstract class BaseModule<T> : Module<string> where T : IModule
              {
                  protected override void Configure(ModuleConfigurationBuilder module) => module.{{(optional ? "DependsOnOptional" : "DependsOn")}}<T>();
              }
              """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Recognizes_Constructed_Dependency_In_Inherited_Configure()
    {
        var source = Source("", "base.Configure(module);", "context.GetModule<GenericDependency<string>>()")
            .Replace("public class Consumer : Module<string>", "public class Consumer : BaseModule<string>")
            + """

              public class GenericDependency<T> : Module<string>
              {
                  protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult("");
              }
              public abstract class BaseModule<T> : Module<string>
              {
                  protected override void Configure(ModuleConfigurationBuilder module) => module.DependsOn<GenericDependency<T>>();
              }
              """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    private static string Source(string attributes, string configure, string access) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}

        public class Dependency : Module<string>
        {
            protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult("");
        }

        {{attributes}}
        public class Consumer : Module<string>
        {
            protected override void Configure(ModuleConfigurationBuilder module) { {{configure}} }

            protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                _ = {{access}};
                return Task.FromResult("");
            }
        }
        """;
}
