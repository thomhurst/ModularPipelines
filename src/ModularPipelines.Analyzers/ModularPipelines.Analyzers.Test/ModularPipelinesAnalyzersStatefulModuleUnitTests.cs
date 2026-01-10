using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<
    ModularPipelines.Analyzers.StatefulModuleAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersStatefulModuleUnitTests
{
    private const string BadModuleWithMutableField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private string {{|#0:_state|}};

    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _state = ""updated"";
        return _state;
    }}
}}
";

    private const string BadModuleWithMutableCollection = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    private List<string> {{|#0:_items|}} = new();

    protected override async Task<int?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _items.Add(""item"");
        return _items.Count;
    }}
}}
";

    private const string BadModuleWithMutableDictionary = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    private Dictionary<string, int> {{|#0:_cache|}} = new();

    protected override async Task<int?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _cache[""key""] = 42;
        return _cache.Count;
    }}
}}
";

    private const string BadModuleWithMutableCounter = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    private int {{|#0:_counter|}};

    protected override async Task<int?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _counter++;
        return _counter;
    }}
}}
";

    private const string GoodModuleWithReadonlyField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private readonly string _config = ""default"";

    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return _config;
    }}
}}
";

    private const string GoodModuleWithReadonlyInjectedDependency = $@"
{TestSourceConstants.StandardModuleHeader}

public interface IMyService {{ }}

public class Module1 : Module<string>
{{
    private readonly IMyService _myService;

    public Module1(IMyService myService)
    {{
        _myService = myService;
    }}

    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return ""done"";
    }}
}}
";

    private const string GoodModuleWithStaticField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private static readonly object Lock = new();

    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return ""done"";
    }}
}}
";

    private const string GoodModuleWithConstField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private const string DefaultValue = ""default"";

    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return DefaultValue;
    }}
}}
";

    private const string GoodModuleWithNoFields = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return ""done"";
    }}
}}
";

    private const string NonModuleClassWithMutableField = @"
#nullable enable
using System;

namespace ModularPipelines.Examples.Other;

public class NotAModule
{
    private string _state;

    public void DoSomething()
    {
        _state = ""updated"";
    }
}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableField()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableField, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCollection()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableCollection, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableDictionary()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableDictionary, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCounter()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableCounter, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_ReadonlyField()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleWithReadonlyField);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_ReadonlyInjectedDependency()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleWithReadonlyInjectedDependency);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_StaticField()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleWithStaticField);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_ConstField()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleWithConstField);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_NoFields()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleWithNoFields);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_NotAModule()
    {
        await VerifyCS.VerifyAnalyzerAsync(NonModuleClassWithMutableField);
    }
}
