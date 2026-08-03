using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.EnumerableModuleResultAnalyzer>;
using VerifyCodeFixCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.EnumerableModuleResultAnalyzer,
    ModularPipelines.Analyzers.EnumerableModuleResultCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersIEnumerableUnitTests
{
    private const string BadModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : {{|#0:Module<IEnumerable<string>>|}}
{{
    protected override async Task<IEnumerable<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return Array.Empty<string>().Select(x => x);
    }}
}}
";

    private const string GoodModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    private const string BadSyncModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : {{|#0:SyncModule<IEnumerable<string>>|}}
{{
    protected override IEnumerable<string>? Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return Array.Empty<string>();
    }}
}}
";

    private const string ForeignModuleSource = $@"
{TestSourceConstants.StandardUsings}

namespace Foreign
{{
    public abstract class Module<T>
    {{
    }}
}}

namespace AnalyzerExamples
{{
    public class Module1 : Foreign.Module<IEnumerable<string>>
    {{
    }}
}}
";

    private const string QualifiedSyncModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : {{|#0:global::ModularPipelines.Modules.SyncModule<System.Collections.Generic.IEnumerable<string>>|}}
{{
    protected override List<string>? Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return [];
    }}
}}
";

    private const string FixedQualifiedSyncModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : global::ModularPipelines.Modules.SyncModule<List<string>>
{{
    protected override List<string>? Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return [];
    }}
}}
";

    private const string WrappedModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public abstract class Wrapper<TMetadata, TResult> : Module<TResult>
{{
}}

public abstract class Module1 : {{|#0:Wrapper<IEnumerable<int>, IEnumerable<string>>|}}
{{
}}
";

    private const string FixedWrappedModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public abstract class Wrapper<TMetadata, TResult> : Module<TResult>
{{
}}

public abstract class Module1 : Wrapper<IEnumerable<int>, List<string>>
{{
}}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_IEnumerable()
    {
        var expected = VerifyCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_List()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_SyncModuleReturnsIEnumerable()
    {
        var expected = VerifyCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadSyncModuleSource, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_ForeignModuleReturnsIEnumerable()
    {
        await VerifyCS.VerifyAnalyzerAsync(ForeignModuleSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Qualified_SyncModule_Wrapper()
    {
        var expected = VerifyCodeFixCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCodeFixCS.VerifyCodeFixAsync(
            QualifiedSyncModuleSource,
            expected,
            FixedQualifiedSyncModuleSource);
    }

    [TestMethod]
    public async Task CodeFix_Replaces_The_Wrapper_Result_Argument()
    {
        var expected = VerifyCodeFixCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCodeFixCS.VerifyCodeFixAsync(
            WrappedModuleSource,
            expected,
            FixedWrappedModuleSource);
    }
}
