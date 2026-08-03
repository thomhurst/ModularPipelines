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
    protected override async Task<IEnumerable<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    protected override IEnumerable<string> Execute(
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
    protected override List<string> Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return [];
    }}
}}
";

    private const string FixedQualifiedSyncModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : global::ModularPipelines.Modules.SyncModule<global::System.Collections.Generic.List<string>>
{{
    protected override List<string> Execute(
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

public abstract class Module1 : Wrapper<IEnumerable<int>, global::System.Collections.Generic.List<string>>
{{
}}
";

    private const string FixedResultWrapperSource = $@"
{TestSourceConstants.StandardModuleHeader}

#pragma warning disable EnumerableModuleResult
public abstract class FixedWrapper<T> : Module<IEnumerable<T>>
{{
}}
#pragma warning restore EnumerableModuleResult

public abstract class Module1 : FixedWrapper<string>
{{
}}
";

    private const string PartialSyncModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public partial class Module1 : SyncModule<IEnumerable<string>>
{{
}}

public partial class Module1
{{
    protected override IEnumerable<string> Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return Array.Empty<string>();
    }}
}}
";

    private const string ResultHookSyncModuleSource = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : SyncModule<IEnumerable<string>>
{{
    protected override List<string> Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return [];
    }}

    protected override Task<ModuleResult<IEnumerable<string>>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<IEnumerable<string>> result,
        CancellationToken cancellationToken)
    {{
        return Task.FromResult<ModuleResult<IEnumerable<string>>?>(result);
    }}
}}
";

    private const string AbstractModuleWithDerivedExecutionSource = $@"
{TestSourceConstants.StandardModuleHeader}

public abstract class BaseModule : Module<IEnumerable<string>>
{{
}}

public class DerivedModule : BaseModule
{{
    protected override Task<IEnumerable<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return Task.FromResult<IEnumerable<string>>([]);
    }}
}}
";

    private const string CollidingListSource = @"
#nullable enable
using System.Collections.Generic;
using System.Threading;
using ModularPipelines.Context;

namespace ModularPipelines.Examples.Modules;

public class List<T>
{
}

public class Module1 : {|#0:global::ModularPipelines.Modules.SyncModule<IEnumerable<string>>|}
{
    protected override global::System.Collections.Generic.List<string> Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return [];
    }
}
";

    private const string FixedCollidingListSource = @"
#nullable enable
using System.Collections.Generic;
using System.Threading;
using ModularPipelines.Context;

namespace ModularPipelines.Examples.Modules;

public class List<T>
{
}

public class Module1 : global::ModularPipelines.Modules.SyncModule<global::System.Collections.Generic.List<string>>
{
    protected override global::System.Collections.Generic.List<string> Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return [];
    }
}
";

    private const string DirectiveElementSource = TestSourceConstants.StandardModuleHeader + """

public abstract class Module1 : {|#0:Module<IEnumerable<
#if STRING_RESULT
    string
#else
    int
#endif
>>|}
{
}
""";

    private const string FixedDirectiveElementSource = TestSourceConstants.StandardModuleHeader + """

public abstract class Module1 : Module<global::System.Collections.Generic.List<
#if STRING_RESULT
    string
#else
    int
#endif
>>
{
}
""";

    private const string NullableEnumerableSource = $@"
{TestSourceConstants.StandardModuleHeader}

public abstract class Module1 : {{|#0:Module<IEnumerable<string>?>|}}
{{
}}
";

    private const string FixedNullableEnumerableSource = $@"
{TestSourceConstants.StandardModuleHeader}

public abstract class Module1 : Module<global::System.Collections.Generic.List<string>?>
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
    public async Task CodeFixIsNotOfferedWhenSyncExecutionReturnWouldBecomeInvalid()
    {
        var source = BadSyncModuleSource
            .Replace("{|#0:", string.Empty)
            .Replace("|}", string.Empty);

        await VerifyCodeFixCS.VerifyNoCodeFixAsync(
            source,
            EnumerableModuleResultAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFixIsNotOfferedWhenPartialExecutionReturnWouldBecomeInvalid()
    {
        await VerifyCodeFixCS.VerifyNoCodeFixAsync(
            PartialSyncModuleSource,
            EnumerableModuleResultAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFixIsNotOfferedWhenResultHookWouldBecomeInvalid()
    {
        await VerifyCodeFixCS.VerifyNoCodeFixAsync(
            ResultHookSyncModuleSource,
            EnumerableModuleResultAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFixIsNotOfferedWhenDerivedExecutionReturnWouldBecomeInvalid()
    {
        await VerifyCodeFixCS.VerifyNoCodeFixAsync(
            AbstractModuleWithDerivedExecutionSource,
            EnumerableModuleResultAnalyzer.DiagnosticId);
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
    public async Task CodeFix_Qualifies_List_When_Local_Type_Collides()
    {
        var expected = VerifyCodeFixCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCodeFixCS.VerifyCodeFixAsync(
            CollidingListSource,
            expected,
            FixedCollidingListSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Element_Type_Directives()
    {
        var expected = VerifyCodeFixCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCodeFixCS.VerifyCodeFixAsync(
            DirectiveElementSource,
            expected,
            FixedDirectiveElementSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Nullable_Result_Annotation()
    {
        var expected = VerifyCodeFixCS.Diagnostic(EnumerableModuleResultAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCodeFixCS.VerifyCodeFixAsync(
            NullableEnumerableSource,
            expected,
            FixedNullableEnumerableSource);
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

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_A_Fixed_Result_Wrapper()
    {
        await VerifyCodeFixCS.VerifyNoCodeFixAsync(
            FixedResultWrapperSource,
            EnumerableModuleResultAnalyzer.DiagnosticId);
    }
}
