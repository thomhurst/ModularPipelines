using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.MissingDependsOnAttributeAnalyzer,
    ModularPipelines.Analyzers.MissingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class SelectorDependencyAnalyzerTests
{
    [TestMethod]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom(typeof(BuildBase))]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<Dependency>]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom(typeof(BuildBase))]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<IBuild>]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom(typeof(GenericBase<>))]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<GenericBase<string>>]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom(typeof(IGenericBuild<>))]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<GenericBase<int>>]", "GetModule", "MP0001")]
    [DataRow("[DependsOnAllModulesInheritingFrom<Other>]", "GetModule", "MP0001")]
    [DataRow("[DependsOnAllModulesInheritingFrom<Other>]", "GetModuleIfRegistered", "MP0001")]
    [DataRow("[DependsOnModulesWithTag(\"build\")]", "GetModule", null)]
    [DataRow("[DependsOnModulesWithTag(\"build\")]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnModulesInCategory(\"build\")]", "GetModule", null)]
    [DataRow("[DependsOnModulesInCategory(\"build\")]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]", "GetModule", null)]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnModulesWithAttribute<AbsentAttribute>]", "GetModule", "MP0001")]
    [DataRow("[DependsOnModulesWithAttribute<NonInheritedAttribute>]", "GetModule", "MP0001")]
    [DataRow("[CustomSelector]", "GetModule", null)]
    [DataRow("[CustomSelector]", "GetModuleIfRegistered", null)]
    [DataRow("[Unrelated.DependsOnAllModulesInheritingFrom<BuildBase>]", "GetModule", "MP0001")]
    public async Task Recognizes_Selectors(string attributes, string accessor, string? diagnosticId)
    {
        var access = $"context.{accessor}<Dependency>()";
        var source = Source(attributes, diagnosticId is null ? access : $"{{|#0:{access}|}}");
        await VerifyCS.VerifyAnalyzerAsync(source, Expected(diagnosticId));
    }

    [TestMethod]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]")]
    [DataRow("[DependsOnModulesWithTag(\"build\")]")]
    [DataRow("[DependsOnModulesInCategory(\"build\")]")]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]")]
    [DataRow("[CustomSelector]")]
    public async Task Recognizes_Inherited_Selectors(string attributes)
    {
        var source = Source("", "context.GetModule<Dependency>()")
            .Replace("public class Consumer : Module<string>", "public class Consumer : ConsumerBase")
            + $$"""

              {{attributes}}
              public abstract class ConsumerBase : Module<string> { }
              """;
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Selector_On_Interface(bool inherited)
    {
        var source = Source("", "context.GetModule<Dependency>()")
            .Replace("public class Consumer : Module<string>", "public class Consumer : Module<string>, IConsumer")
            + $$"""

              [DependsOnAllModulesInheritingFrom<BuildBase>]
              public interface ISelector { }
              public interface IConsumer {{(inherited ? ": ISelector" : "")}} { }
              """;
        if (!inherited)
        {
            source = source.Replace("public interface IConsumer", "[DependsOnAllModulesInheritingFrom<BuildBase>] public interface IConsumer");
        }

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("[DependsOnModulesWithTag(\"build\")]", "GetModule", null)]
    [DataRow("[DependsOnModulesInCategory(\"build\")]", "GetModule", null)]
    [DataRow("[CustomSelector]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]", "GetModule", null)]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<Other>]", "GetModule", "MP0021")]
    public async Task Merges_Selectors_With_Optional_Declarations(string selector, string accessor, string? diagnosticId)
    {
        var access = $"context.{accessor}<Dependency>()";
        await VerifyCS.VerifyAnalyzerAsync(Source(selector + "[DependsOn<Dependency>(Optional = true)]",
            diagnosticId is null ? access : $"{{|#0:{access}|}}"), Expected(diagnosticId));
    }

    [TestMethod]
    [DataRow("[CustomSelector]")]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]")]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]")]
    public async Task Preserves_Configured_Required_Declaration_With_Selector(string selector)
    {
        var source = Source(selector, "{|#0:context.GetModuleIfRegistered<Dependency>()|}", "module.DependsOn<Dependency>();");
        await VerifyCS.VerifyAnalyzerAsync(source, Expected("MP0022"));
    }

    [TestMethod]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]")]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]")]
    public async Task Preserves_Attribute_Required_Declaration_With_Selector(string selector)
    {
        var source = Source(selector + "[DependsOn<Dependency>]", "{|#0:context.GetModuleIfRegistered<Dependency>()|}");
        await VerifyCS.VerifyAnalyzerAsync(source, Expected("MP0022"));
    }

    [TestMethod]
    public async Task Does_Not_Select_Consumer_Itself()
    {
        var source = Source("[CustomSelector]", "{|#0:context.GetModule<Consumer>()|}");
        await VerifyCS.VerifyAnalyzerAsync(source, new DiagnosticResult("MP0001", DiagnosticSeverity.Error)
            .WithLocation(0).WithArguments("Consumer"));
    }

    [TestMethod]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]", "GetModule")]
    [DataRow("[DependsOnAllModulesInheritingFrom<BuildBase>]", "GetModuleIfRegistered")]
    [DataRow("[DependsOnModulesWithAttribute<MarkerAttribute>]", "GetModuleIfRegistered")]
    public async Task Does_Not_Offer_Redundant_DependsOn_CodeFix(string selector, string accessor)
    {
        var source = Source(selector, $"context.{accessor}<Dependency>()");
        await VerifyCS.VerifyCodeFixAsync(source, source);
    }

    [TestMethod]
    [DataRow("[DependsOnAllModulesInheritingFrom<IGenericBuild<object>>]", "GetModule", null)]
    [DataRow("[DependsOnAllModulesInheritingFrom<IGenericBuild<object>>]", "GetModuleIfRegistered", null)]
    [DataRow("[DependsOnModulesWithAttribute<Attribute>]", "GetModule", null)]
    [DataRow("[DependsOnModulesWithAttribute<Attribute>]", "GetModuleIfRegistered", null)]
    public async Task Recognizes_Assignable_Selector_Targets(string selector, string accessor, string? diagnosticId)
    {
        var source = Source(selector, $"context.{accessor}<Dependency>()")
            .Replace("interface IGenericBuild<T>", "interface IGenericBuild<out T>");
        await VerifyCS.VerifyAnalyzerAsync(source, Expected(diagnosticId));
    }

    [TestMethod]
    [DataRow("T")]
    [DataRow("T[]")]
    public async Task Does_Not_Disprove_Unresolved_Generic_Matches(string argument)
    {
        var source = Source("[DependsOnAllModulesInheritingFrom<GenericBase<string>>]",
                $"context.GetModule<GenericDependency<{argument}>>()")
            .Replace("public class Consumer : Module<string>", "public class Consumer<T> : Module<string>")
            + """

              public class GenericDependency<T> : GenericBase<T>
              {
                  protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult("");
              }
              """;
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Inherit_NonInherited_Custom_Selector()
    {
        var source = Source("", "{|#0:context.GetModule<Dependency>()|}")
            .Replace("public class CustomSelectorAttribute", "[AttributeUsage(AttributeTargets.Class, Inherited = false)] public class CustomSelectorAttribute")
            .Replace("public class Consumer : Module<string>", "public class Consumer : ConsumerBase")
            + """

              [CustomSelector]
              public abstract class ConsumerBase : Module<string> { }
              """;
        await VerifyCS.VerifyAnalyzerAsync(source, Expected("MP0001"));
    }

    private static DiagnosticResult[] Expected(string? diagnosticId) => diagnosticId is null ? [] :
    [
        new DiagnosticResult(diagnosticId, diagnosticId switch
        {
            "MP0021" => DiagnosticSeverity.Warning,
            "MP0022" => DiagnosticSeverity.Info,
            _ => DiagnosticSeverity.Error,
        }).WithLocation(0).WithArguments("Dependency"),
    ];

    private static string Source(string attributes, string access, string configure = "") => $$"""
        {{TestSourceConstants.StandardModuleHeader}}

        public interface IBuild : IModule { }
        public interface IGenericBuild<T> : IModule { }
        public abstract class GenericBase<T> : Module<string>, IGenericBuild<T> { }
        [Marker, NonInherited]
        public abstract class BuildBase : GenericBase<string>, IBuild { }
        public class Dependency : BuildBase
        {
            protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.FromResult("");
        }
        public abstract class Other : Module<string> { }
        public class MarkerAttribute : Attribute { }
        public class AbsentAttribute : Attribute { }
        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        public class NonInheritedAttribute : Attribute { }
        public class CustomSelectorAttribute : DependsOnBaseAttribute
        {
            public override bool ShouldDependOn(Type candidateModule, ModularPipelines.Context.IDependencyContext context) => DateTime.Now.Ticks > 0;
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
        public static class Unrelated
        {
            public class DependsOnAllModulesInheritingFromAttribute<T> : Attribute { }
        }
        """;
}
