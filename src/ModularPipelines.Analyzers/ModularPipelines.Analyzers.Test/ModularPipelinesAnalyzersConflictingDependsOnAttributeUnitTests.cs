using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularPipelines;
using System.Text;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ConflictingDependsOnAttributeAnalyzer>;
using VerifyCodeFixCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.ConflictingDependsOnAttributeAnalyzer,
    ModularPipelines.Analyzers.ConflictingDependsOnAttributeCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersConflictingDependsOnAttributeUnitTests
{
    private const string SimpleModuleBody = @"
{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}";

    private const string BadModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module2>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[{{|#1:DependsOn<Module1>|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    private const string BadModuleSource2 = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module1>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string IndirectCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module2>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[{{|#1:DependsOn<Module3>|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}

[{{|#2:DependsOn<Module1>|}}]
public class Module3 : Module<List<string>>
{SimpleModuleBody}
";

    private const string NonGenericIndirectCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn(typeof(Module2))|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[{{|#1:DependsOn(typeof(Module3))|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}

[{{|#2:DependsOn(typeof(Module1))|}}]
public class Module3 : Module<List<string>>
{SimpleModuleBody}
";

    private const string AcyclicChainSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[DependsOn<Module2>]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[DependsOn<Module3>]
public class Module2 : Module<List<string>>
{SimpleModuleBody}

public class Module3 : Module<List<string>>
{SimpleModuleBody}
";

    private const string InheritedInterfaceCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

    [{{|#0:DependsOn<Module2>|}}]
public interface IModule1Dependencies
{{
}}

public class Module1 : Module<List<string>>, IModule1Dependencies
{SimpleModuleBody}

[{{|#1:DependsOn<Module1>|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    private const string InheritedBaseCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module2>|}}]
public abstract class Module1Base : Module<List<string>>
{{
}}

public class Module1 : Module1Base
{SimpleModuleBody}

[{{|#1:DependsOn<Module1>|}}]
public abstract class Module2Base : Module<List<string>>
{{
}}

public class Module2 : Module2Base
{SimpleModuleBody}
";

    private const string ConstructedGenericCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module2>|}}]
public class Module1<T> : Module<List<string>>
{SimpleModuleBody}

[{{|#1:DependsOn<Module1<int>>|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    private const string GeneratedCycleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[{{|#0:DependsOn<Module2>|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}
";

    private const string GeneratedDependencySource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[DependsOn<Module1>]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    private const string GoodModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{SimpleModuleBody}

[DependsOn<Module1>]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    private const string AliasedCycleSource = $@"
{TestSourceConstants.StandardUsingsWithLogging}
using Dependency = ModularPipelines.DependsOnAttribute;

{TestSourceConstants.ExamplesNamespace}

[{{|#0:Dependency(typeof(Module2))|}}]
public class Module1 : Module<List<string>>
{SimpleModuleBody}

[{{|#1:Dependency(typeof(Module1))|}}]
public class Module2 : Module<List<string>>
{SimpleModuleBody}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Conflicting_Dependencies()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module1", "Module2");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected1, expected2);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Dependency_Depends_On_Self()
    {
        var expected = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module1", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource2, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Indirect_Cycle_Exists()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module3", "Module2");

        var expected3 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(2)
            .WithArguments("Module1", "Module3");

        await VerifyCS.VerifyAnalyzerAsync(IndirectCycleSource, expected1, expected2, expected3);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_NonGeneric_Indirect_Cycle_Exists()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module3", "Module2");

        var expected3 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(2)
            .WithArguments("Module1", "Module3");

        await VerifyCS.VerifyAnalyzerAsync(
            NonGenericIndirectCycleSource,
            expected1,
            expected2,
            expected3);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_Indirect_Dependency_Chain_Is_Acyclic()
    {
        await VerifyCS.VerifyAnalyzerAsync(AcyclicChainSource);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Cycle_Uses_Inherited_Interface_Dependency()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module1", "Module2");

        await VerifyCS.VerifyAnalyzerAsync(
            InheritedInterfaceCycleSource,
            expected1,
            expected2);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_All_Cycle_Edges_Are_Inherited()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module1", "Module2");

        await VerifyCS.VerifyAnalyzerAsync(
            InheritedBaseCycleSource,
            expected1,
            expected2);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Cycle_Uses_Constructed_Generic()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1<T>");

        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module1<int>", "Module2");

        await VerifyCS.VerifyAnalyzerAsync(
            ConstructedGenericCycleSource,
            expected1,
            expected2);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Cycle_Uses_Generated_Dependency()
    {
        var expected = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");
        var test = new GeneratedDependencyAnalyzerTest
        {
            TestCode = GeneratedCycleSource,
            ReferenceAssemblies = Net.Net100,
            TestState =
            {
                AdditionalReferences = { typeof(IModuleContext).Assembly.Location },
            },
        };

        test.TestState.GeneratedSources.Add((
            typeof(GeneratedDependencySourceGenerator),
            "GeneratedDependency.g.cs",
            SourceText.From(GeneratedDependencySource, Encoding.UTF8)));
        test.ExpectedDiagnostics.Add(expected);

        await test.RunAsync(CancellationToken.None);
    }

    [TestMethod]
    public async Task AnalyzerHandlesLongDependencyChainsIteratively()
    {
        await VerifyCS.VerifyAnalyzerAsync(CreateLongDependencyChain(5_000));
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_No_Conflicting_Dependencies()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Cycle_UsesAttributeAlias()
    {
        var expected1 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("Module2", "Module1");
        var expected2 = VerifyCS.Diagnostic(ConflictingDependsOnAttributeAnalyzer.DiagnosticId)
            .WithLocation(1)
            .WithArguments("Module1", "Module2");

        await VerifyCS.VerifyAnalyzerAsync(AliasedCycleSource, expected1, expected2);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Attribute_List_Contains_Directives()
    {
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithLogging}}

            public class Module1 : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }

            [
            #if true
            DependsOn<Module2>
            #else
            Obsolete
            #endif
            ]
            public class Module2 : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
            }
            """;

        await VerifyCodeFixCS.VerifyNoCodeFixAsync(
            source,
            ConflictingDependsOnAttributeAnalyzer.DiagnosticId);
    }

    private static string CreateLongDependencyChain(int length)
    {
        var source = new StringBuilder(
            TestSourceConstants.StandardModuleHeaderWithLogging);
        source.AppendLine("""
                          public abstract class ChainModule : Module<List<string>>
                          {
                              protected override Task<List<string>> ExecuteAsync(
                                  IModuleContext context,
                                  CancellationToken cancellationToken)
                              {
                                  return Task.FromResult<List<string>>([]);
                              }
                          }
                          """);

        for (var index = 0; index < length - 1; index++)
        {
            source.AppendLine($"[DependsOn<Chain{index + 1}>]");
            source.AppendLine($"public class Chain{index} : ChainModule");
            source.AppendLine("{");
            source.AppendLine("}");
        }

        source.AppendLine($"public class Chain{length - 1} : ChainModule");
        source.AppendLine("{");
        source.AppendLine("}");

        return source.ToString();
    }

    private sealed class GeneratedDependencyAnalyzerTest : VerifyCS.Test
    {
        protected override IEnumerable<Type> GetSourceGenerators()
        {
            return [typeof(GeneratedDependencySourceGenerator)];
        }
    }

    public sealed class GeneratedDependencySourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("GeneratedDependency.g.cs", GeneratedDependencySource);
        }
    }
}
