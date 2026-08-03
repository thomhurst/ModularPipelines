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

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    private const string ModuleWithImmutableField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private string {{|#0:_name|}};

    public Module1()
    {{
        _name = ""module"";
    }}

    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<string>(_name);
    }}
}}
";

    private const string FixedModuleWithImmutableField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<string>
{{
    private readonly string _name;

    public Module1()
    {{
        _name = ""module"";
    }}

    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<string>(_name);
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

    private const string BadModuleWithMutableAutoProperty = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    public int {{|#0:Counter|}} {{ get; set; }}

    protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult(Counter);
    }}
}}
";

    private const string BadModuleWithRequiredMutableAutoProperty = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    public required int {{|#0:Counter|}} {{ get; set; }}

    protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult(Counter);
    }}
}}
";

    private const string PartialModuleWithMutableField = $@"
{TestSourceConstants.StandardModuleHeader}

public partial class Module1 : Module<int>
{{
    private int {{|#0:_counter|}};

    protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult(_counter);
    }}
}}

public partial class Module1
{{
}}
";

    private const string GoodModuleWithNonWritableProperties = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    public int GetterOnly {{ get; }}

    public int InitOnly {{ get; init; }}

    public required int Required {{ get; init; }}

    public static int Static {{ get; set; }}

    protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult(GetterOnly + InitOnly + Required + Static);
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

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<string>(_state);
    }
}
";

    private const string ModuleWithOtherInstanceConstructorWrite = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<string>
{
    private string _state = string.Empty;

    public Module1(Module1 other)
    {
        other._state = ""updated"";
    }

    protected override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<string>(_state);
    }
}
";

    private const string ModuleWithObjectInitializerConstructorWrite = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<string>
{
    private string _state = string.Empty;

    public Module1()
    {
        _ = new Module1(true)
        {
            _state = ""updated"",
        };
    }

    private Module1(bool _) { }

    protected override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<string>(_state);
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

    private const string ModuleWithRefExtensionReceiver = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public static class IntExtensions
{
    public static void Increment(this ref int value)
    {
        value++;
    }
}

public class Module1 : Module<int>
{
    private int _state;

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        _state.Increment();
        return Task.FromResult(_state);
    }
}
";

    private const string ModuleWithInExtensionReceiver = @"
#nullable enable
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public static class IntExtensions
{
    public static void Increment(this in int value)
    {
        Unsafe.AsRef(in value)++;
    }
}

public class Module1 : Module<int>
{
    private int _state;

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        _state.Increment();
        return Task.FromResult(_state);
    }
}
";

    private const string ModuleWithInactiveFieldWrite = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<int>
{
    private int _state;

#if MUTATE
    private void Mutate()
    {
        _state = 1;
    }
#endif

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_state);
    }
}
";

    private const string ModuleWithImplicitInArgument = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<int>
{
    private int _state;

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        Escape(_state);
        return Task.FromResult(_state);
    }

    private static void Escape(in int value)
    {
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

    protected override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<string>(_state);
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

    protected override Task<object> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<object>(_state);
    }
}
";

    private const string ModuleWithRequiredField = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<object>
{
    public required object State;

    protected override Task<object> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return Task.FromResult<object>(State);
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

    protected override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        (_state, _) = (""updated"", 0);
        return Task.FromResult<string>(_state);
    }
}
";

    private const string ModuleWithExternallyWritableField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    public List<string> _items = new();

    protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult(_items.Count);
    }}
}}

public static class ModuleConfigurator
{{
    public static void Configure(Module1 module)
    {{
        module._items = [];
    }}
}}
";

    private const string NestedModuleWithEnclosingWrite = $@"
{TestSourceConstants.StandardModuleHeader}

public class ModuleContainer
{{
    public class Module1 : Module<int>
    {{
        private int _state;

        protected override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {{
            return Task.FromResult(_state);
        }}
    }}

    public static void Configure(Module1 module)
    {{
        module._state = 1;
    }}
}}
";

    private const string BadModuleWithReadonlyStructContainingMutableState = $@"
{TestSourceConstants.StandardModuleHeader}

public readonly struct CacheHolder
{{
    public List<string> Items {{ get; }} = new();

    public CacheHolder()
    {{
    }}
}}

public class Module1 : Module<int>
{{
    private CacheHolder {{|#0:_cache|}} = new();

    protected override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        _cache.Items.Add(""item"");
        return Task.FromResult(_cache.Items.Count);
    }}
}}
";

    private const string ModuleWithRefEscapedField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<object>
{{
    private object _state = new();

    private ref object State => ref _state;

    protected override Task<object> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<object>(State);
    }}
}}
";

    private const string ModuleWithAddressOfField = @"
#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Context;
using ModularPipelines.Modules;

public class Module1 : Module<int>
{
    private int _state;

    protected override unsafe Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        fixed (int* pointer = &_state)
        {
            return Task.FromResult(*pointer);
        }
    }
}
";

    private const string ModuleWithMakeRefField = $@"
{TestSourceConstants.StandardModuleHeader}

public class Module1 : Module<int>
{{
    private int _state;

    protected override Task<int> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        var reference = __makeref(_state);
        return Task.FromResult(__refvalue(reference, int));
    }}
}}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableField()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.Rule).WithLocation(0).WithArguments("_state", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableField, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCollection()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.Rule).WithLocation(0).WithArguments("_items", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableCollection, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableDictionary()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.Rule).WithLocation(0).WithArguments("_cache", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableDictionary, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCounter()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.Rule).WithLocation(0).WithArguments("_counter", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableCounter, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableAutoProperty()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.PropertyRule)
            .WithLocation(0)
            .WithArguments("Counter", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(BadModuleWithMutableAutoProperty, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_RequiredAutoPropertyHasSetter()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.PropertyRule)
            .WithLocation(0)
            .WithArguments("Counter", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(
            BadModuleWithRequiredMutableAutoProperty,
            expected);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Mutable_Auto_Property()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleWithMutableAutoProperty
                .Replace("{|#0:", string.Empty)
                .Replace("|}", string.Empty),
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task AnalyzerReportsMutableFieldOnce_When_ModuleIsPartial()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.Rule)
            .WithLocation(0)
            .WithArguments("_counter", "Module1");

        await VerifyCS.VerifyAnalyzerAsync(PartialModuleWithMutableField, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_PropertiesCannotLeakMutableState()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleWithNonWritableProperties);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_MutableCustomClass()
    {
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.Rule).WithLocation(0).WithArguments("_cache", "Module1");

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
        var expected = VerifyCS.Diagnostic(StatefulModuleAnalyzer.Rule)
            .WithLocation(0)
            .WithArguments("_name", "Module1");

        await VerifyCS.VerifyCodeFixAsync(
            ModuleWithImmutableField,
            expected,
            FixedModuleWithImmutableField);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Mutable_Reference_Type()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleWithMutableCollection
                .Replace("{|#0:", string.Empty)
                .Replace("|}", string.Empty),
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Readonly_Struct_With_Mutable_State()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BadModuleWithReadonlyStructContainingMutableState
                .Replace("{|#0:", string.Empty)
                .Replace("|}", string.Empty),
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Nullable_Struct_With_Mutable_State()
    {
        var source = BadModuleWithReadonlyStructContainingMutableState
            .Replace("private CacheHolder {|#0:_cache|}", "private CacheHolder? _cache")
            .Replace("_cache.Items", "_cache.Value.Items");

        await VerifyCS.VerifyNoCodeFixAsync(
            source,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Constructor_Nested_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithConstructorNestedWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Other_Instance_Constructor_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithOtherInstanceConstructorWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Object_Initializer_Constructor_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithObjectInitializerConstructorWrite,
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
    public async Task CodeFix_Is_Not_Offered_For_Required_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithRequiredField,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Deconstruction_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithDeconstructionWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Externally_Writable_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithExternallyWritableField,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Ref_Extension_Receiver()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithRefExtensionReceiver,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_In_Extension_Receiver()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithInExtensionReceiver,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Inactive_Field_Write()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithInactiveFieldWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Implicit_In_Argument()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithImplicitInArgument,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Nested_Module()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            NestedModuleWithEnclosingWrite,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Ref_Escaped_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithRefEscapedField,
            StatefulModuleAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Address_Of_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithAddressOfField,
            StatefulModuleAnalyzer.DiagnosticId,
            allowUnsafe: true);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_MakeRef_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ModuleWithMakeRefField,
            StatefulModuleAnalyzer.DiagnosticId);
    }
}
