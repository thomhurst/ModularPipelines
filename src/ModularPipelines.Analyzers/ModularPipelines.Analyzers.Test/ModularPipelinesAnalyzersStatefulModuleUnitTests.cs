using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.StatefulModuleAnalyzer,
    ModularPipelines.Analyzers.StatefulModuleCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersStatefulModuleUnitTests
{
    private const string BadModuleWithMutableField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private string {{|#0:_state|}} = string.Empty;

    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _items.Add(""item"");
        return _items.Count;
    }}
}}
";

    private const string FixedModuleWithReadonlyCollection = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    private readonly List<string> _items = new();

    protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _counter++;
        return _counter;
    }}
}}
";

    private const string BadModuleWithMutableCustomClass = $@"
{TestSourceConstants.StandardModuleHeader}

public class MyCache
{{
    public Dictionary<string, object> Items {{ get; }} = new();
}}

public class Module1 : Module<int>
{{
    private MyCache {{|#0:_cache|}} = new();

    protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _cache.Items[""key""] = ""value"";
        return _cache.Items.Count;
    }}
}}
";

    private const string GoodModuleWithReadonlyField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private readonly string _config = ""default"";

    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    protected override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    private string _state = string.Empty;

    public void DoSomething()
    {
        _state = ""updated"";
    }
}
";

    private const string ModuleWithConstructorNestedWrite = @"
#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<string>
{
    private string _state = string.Empty;

    public Module1()
    {
        void SetState()
        {
            _state = ""updated"";
        }
    }

    protected override Task<string?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(_state);
    }
}
";

    private const string ModuleWithMutableStructMethodCall = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public struct Counter
{
    public int Value { get; private set; }

    public void Increment()
    {
        Value++;
    }
}

public class Module1 : Module<int>
{
    private Counter _counter;

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        _counter.Increment();
        return Task.FromResult(_counter.Value);
    }
}
";

    private const string ModuleWithMutableStructMemberAssignment = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public struct Counter
{
    public int Value { get; set; }
}

public class Module1 : Module<int>
{
    private Counter _counter;

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        _counter.Value = 1;
        return Task.FromResult(_counter.Value);
    }
}
";

    private const string PartialModuleWithWrite = @"
#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public partial class Module1 : Module<string>
{
    private string _state = string.Empty;

    protected override Task<string?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(_state);
    }
}

public partial class Module1
{
    public void SetState()
    {
        _state = ""updated"";
    }
}
";

    private const string ModuleWithVolatileField = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<object>
{
    private volatile object _state = new();

    protected override Task<object?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<object?>(_state);
    }
}
";

    private const string ModuleWithDeconstructionWrite = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<string>
{
    private string _state = string.Empty;

    protected override Task<string?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        (_state, _) = (""updated"", 0);
        return Task.FromResult<string?>(_state);
    }
}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableField()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0).WithArguments("_state", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableField, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCollection()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0).WithArguments("_items", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableCollection, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableDictionary()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0).WithArguments("_cache", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableDictionary, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCounter()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0).WithArguments("_counter", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableCounter, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCustomClass()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId).WithLocation(0).WithArguments("_cache", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableCustomClass, expected);
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

    [TestMethod]
    public async Task CodeFix_Makes_Eligible_Field_Readonly()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.DiagnosticId)
            .WithLocation(0)
            .WithArguments("_items", "Module1");

        await VerifyCS.VerifyCodeFixAsync(
            BadModuleWithMutableCollection,
            expected,
            FixedModuleWithReadonlyCollection);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Constructor_Nested_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithConstructorNestedWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Partial_Type()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            PartialModuleWithWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Mutable_Struct_Method_Call()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithMutableStructMethodCall,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Mutable_Struct_Member_Assignment()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithMutableStructMemberAssignment,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Volatile_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithVolatileField,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Deconstruction_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithDeconstructionWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }
}
