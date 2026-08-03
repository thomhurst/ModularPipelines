using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Analyzers.LoggerInConstructorAnalyzer,
    ModularPipelines.Analyzers.LoggerInConstructorCodeFixProvider>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersILoggerUnitTests
{
    private static string CreateModuleWithLoggerConstructor(string constructorParam) => $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1({{|#0:{constructorParam}|}})
    {{
    }}

    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    private static readonly string BadModuleSourceILogger = CreateModuleWithLoggerConstructor("ILogger logger");
    private static readonly string BadModuleSourceILoggerProvider = CreateModuleWithLoggerConstructor("ILoggerProvider loggerProvider");
    private static readonly string BadModuleSourceILoggerFactory = CreateModuleWithLoggerConstructor("ILoggerFactory loggerFactory");
    private static readonly string BadModuleSourceILoggerGeneric = CreateModuleWithLoggerConstructor("ILogger<Module1> logger");
    private static readonly string AttributedLoggerParameterSource =
        CreateModuleWithLoggerConstructor("[System.Diagnostics.CodeAnalysis.AllowNull] ILogger<Module1> logger")
            .Replace("{|#0:", string.Empty)
            .Replace("|}", string.Empty);

    private const string FixedModuleSourceILoggerGeneric = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    private const string MultipleLoggerParametersSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1(
        {{|#0:ILogger<Module1> logger|}},
        {{|#1:ILoggerFactory loggerFactory|}})
    {{
    }}

    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    private const string AbstractModuleSourceILoggerGeneric = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public abstract class Module1 : Module<List<string>>
{{
    public Module1({{|#0:ILogger<Module1> logger|}})
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedAbstractModuleSourceILoggerGeneric = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public abstract class Module1 : Module<List<string>>
{{
    public Module1()
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string BadModuleSourceUsedLogger = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1({{|#0:ILogger<Module1> logger|}})
    {{
        _logger = logger;
    }}

    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _logger.LogInformation(""Running"");
        return new List<string>();
    }}
}}
";

    private const string FixedModuleSourceUsedLogger = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        context.Logger.LogInformation(""Running"");
        return new List<string>();
    }}
}}
";

    private const string LoggerFieldWithInitializerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private ILogger<Module1> _logger = RegisterTelemetry();

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}

    private static ILogger<Module1> RegisterTelemetry() => null!;
}}
";

    private const string AttributedLoggerFieldSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

[AttributeUsage(AttributeTargets.Field)]
public sealed class PreserveAttribute : Attribute
{{
}}

public class Module1 : Module<List<string>>
{{
    [Preserve]
    private readonly ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string AttributedLoggerConstructorSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    [Obsolete(""Kept for compatibility"")]
    public Module1({{|#0:ILogger<Module1> logger|}})
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string ShadowedContextStoredLoggerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        Action<int> log = context => _logger.LogInformation(""Running"");
        log(0);
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedAttributedLoggerConstructorSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    [Obsolete(""Kept for compatibility"")]
    public Module1()
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string DirectiveLoggerConstructorSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1({{|#0:ILogger<Module1> logger|}})
    {{
#pragma warning disable CS0618
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedDirectiveLoggerConstructorSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1()
    {{
#pragma warning disable CS0618
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string DirectiveLoggerParameterListSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1(
#region logger parameter
        ILogger<Module1> logger
#endregion
    )
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string DirectiveLoggerFieldSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
#if true
    private readonly ILogger<Module1> _logger;
    private readonly int _marker;
#endif

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string DirectiveLoggerAssignmentSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
#if true
        _logger = logger;
        RegisterTelemetry();
#endif
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}

    private static void RegisterTelemetry()
    {{
    }}
}}
";

    private const string InactiveLoggerReferenceSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

#if EXTRA_LOGGING
    private void LogHidden()
    {{
        _logger.LogInformation(""Hidden"");
    }}
#endif

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string BadPrivateConstructorSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private Module1({{|#0:ILogger<Module1> logger|}})
    {{
    }}

    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    private const string FixedPrivateConstructorSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private Module1()
    {{
    }}

    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    private const string PartialModuleSourceUsedLogger = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public partial class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}
}}

public partial class Module1
{{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        _logger.LogInformation(""Running"");
        return new List<string>();
    }}
}}
";

    private const string ConstructorInitializerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private static ILogger<Module1> Logger {{ get; }} = null!;

    public Module1()
        : this(Logger, 1)
    {{
    }}

    private Module1(ILogger<Module1> logger, int value)
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string ExplicitConstructorCallSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1(ILogger<Module1> logger)
    {{
    }}

    public static Module1 Create(ILogger<Module1> logger) => new(logger);

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string BaseConstructorCallSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1(ILogger<Module1> logger)
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}

public class DerivedModule : Module1
{{
    private static ILogger<Module1> Logger {{ get; }} = null!;

    public DerivedModule()
        : base(Logger)
    {{
    }}
}}
";

    private const string DuplicateConstructorSignatureSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1(ILogger<Module1> logger, int value)
    {{
    }}

    public Module1(int value)
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string LoggerConstructorWithOverloadSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1({{|#0:ILogger<Module1> logger|}})
    {{
    }}

    public Module1(int value)
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedLoggerConstructorWithOverloadSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1()
    {{
    }}

    public Module1(int value)
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string LoggerConstructorWithOptionalOverloadSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public Module1(ILogger<Module1> logger)
    {{
    }}

    public Module1(int value = 0)
    {{
    }}

    public static Module1 Create() => new();

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string EmbeddedLoggerAssignmentSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private ILogger<Module1> _logger = null!;

    public Module1(ILogger<Module1> logger, bool enabled)
    {{
        if (enabled)
            _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string EscapedContextStoredLoggerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1({{|#0:ILogger<Module1> logger|}})
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string FixedEscapedContextStoredLoggerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        @event.Logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string StaticLocalFunctionStoredLoggerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private static ILogger<Module1> _logger = null!;

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        static void Log()
        {{
            _logger.LogInformation(""Running"");
        }}

        Log();
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string ExternallyReferencedStoredLoggerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    public readonly ILogger<Module1> Logger;

    public Module1(ILogger<Module1> logger)
    {{
        Logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>>([]);
    }}
}}

public static class LoggerConsumer
{{
    public static void Log(Module1 module)
    {{
        module.Logger.LogInformation(""Running"");
    }}
}}
";

    private const string NestedModuleLoggerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class ModuleContainer
{{
    public class Module1 : Module<List<string>>
    {{
        private readonly ILogger<Module1> _logger;

        public Module1(ILogger<Module1> logger)
        {{
            _logger = logger;
        }}

        protected override Task<List<string>> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {{
            return Task.FromResult<List<string>>([]);
        }}
    }}

    public static void Log(Module1 module)
    {{
        module._logger.LogInformation(""Running"");
    }}
}}
";

    private const string LoggerWithGenericExtensionSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public static class LoggerExtensions
{{
    public static void LogModuleSpecific(this ILogger<Module1> logger)
    {{
    }}
}}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        _logger.LogModuleSpecific();
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string LoggerWithMoreSpecificExtensionSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}
using ModularPipelines.Logging;

public static class LoggerExtensions
{{
    public static void LogCustom(this ILogger logger)
    {{
    }}

    public static void LogCustom(this IModuleLogger logger)
    {{
    }}
}}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger _logger;

    public Module1(ILogger logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        _logger.LogCustom();
        return Task.FromResult<List<string>>([]);
    }}
}}
";

    private const string GoodModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    private const string GoodModuleSource2 = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}
using ModularPipelines.Logging;

public class Module1 : Module<List<string>>
{{
    public Module1(IModuleLoggerProvider moduleLoggerProvider)
    {{
    }}

    protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }}
}}
";

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_ILogger_In_Constructor()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSourceILogger, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_ILoggerGeneric_In_Constructor()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSourceILoggerGeneric, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_ILoggerFactory_In_Constructor()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSourceILoggerFactory, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_ILoggerProvider_In_Constructor()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSourceILoggerProvider, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsTriggered_When_ILogger_In_Primary_Constructor()
    {
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1({{|#0:ILogger<Module1> logger|}}) : Module<List<string>>
{{
    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
        => Task.FromResult<List<string>>([]);
}}
";
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId)
            .WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_For_Non_Module_Constructors()
    {
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Service
{{
    public Service(ILogger<Service> logger)
    {{
    }}
}}

public class PrimaryService(ILogger<PrimaryService> logger)
{{
}}

public class Module1 : Module<List<string>>
{{
    private class NestedService(ILogger<NestedService> logger)
    {{
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
        => Task.FromResult<List<string>>([]);
}}
";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_No_Logger_In_Constructor()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource);
    }

    [TestMethod]
    public async Task AnalyzerIsNotTriggered_When_No_Logger_In_Constructor2()
    {
        await VerifyCS.VerifyAnalyzerAsync(GoodModuleSource2);
    }

    [TestMethod]
    public async Task CodeFix_Removes_Unused_Logger_Parameter()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(BadModuleSourceILoggerGeneric, expected, FixedModuleSourceILoggerGeneric);
    }

    [TestMethod]
    public async Task CodeFix_Removes_CoLocated_Logger_Parameters()
    {
        var expected = new[]
        {
            VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0),
            VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(1),
        };

        await VerifyCS.VerifyCodeFixAsync(
            MultipleLoggerParametersSource,
            expected,
            FixedModuleSourceILoggerGeneric);
    }

    [TestMethod]
    public async Task CodeFix_Retains_Public_Constructor_On_Abstract_Type()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            AbstractModuleSourceILoggerGeneric,
            expected,
            FixedAbstractModuleSourceILoggerGeneric);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Attributed_Constructor()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            AttributedLoggerConstructorSource,
            expected,
            FixedAttributedLoggerConstructorSource);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Constructor_Containing_Directives()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            DirectiveLoggerConstructorSource,
            expected,
            FixedDirectiveLoggerConstructorSource);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Parameter_List_Contains_Directives()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            DirectiveLoggerParameterListSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Logger_Parameter_Has_Attributes()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            AttributedLoggerParameterSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Logger_Field_Contains_Directives()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            DirectiveLoggerFieldSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Logger_Assignment_Contains_Directives()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            DirectiveLoggerAssignmentSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Inactive_Logger_Reference()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            InactiveLoggerReferenceSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Logger_Field_Has_Attributes()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            AttributedLoggerFieldSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Replaces_Stored_Logger_With_Context_Logger()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(BadModuleSourceUsedLogger, expected, FixedModuleSourceUsedLogger);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Private_Constructor_Accessibility()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            BadPrivateConstructorSource,
            expected,
            FixedPrivateConstructorSource);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Partial_Type()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            PartialModuleSourceUsedLogger,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_This_Initializer_Targets_Constructor()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ConstructorInitializerSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Constructor_Is_Called_Explicitly()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ExplicitConstructorCallSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Base_Constructor_Is_Called()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            BaseConstructorCallSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Resulting_Constructor_Would_Be_Duplicate()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            DuplicateConstructorSignatureSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Retains_Public_Constructor_When_Another_Overload_Remains()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            LoggerConstructorWithOverloadSource,
            expected,
            FixedLoggerConstructorWithOverloadSource);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Sibling_Constructor_Arity_Overlaps()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            LoggerConstructorWithOptionalOverloadSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Embedded_Logger_Assignment()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            EmbeddedLoggerAssignmentSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Other_Instance_Logger_Assignment()
    {
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private readonly ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
        GetOther()._logger = logger;
    }}

    private static Module1 GetOther() => null!;

    protected override async Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return [];
    }}
}}
";

        await VerifyCS.VerifyNoCodeFixAsync(
            source,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Static_Logger_Field()
    {
        var source = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    private static ILogger<Module1> _logger;

    public Module1(ILogger<Module1> logger)
    {{
        _logger = logger;
    }}

    protected override Task<List<string>> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>>([]);
    }}
}}
";

        await VerifyCS.VerifyNoCodeFixAsync(
            source,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Preserves_Escaped_Context_Identifier()
    {
        var expected = VerifyCS.Diagnostic(LoggerInConstructorAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyCodeFixAsync(
            EscapedContextStoredLoggerSource,
            expected,
            FixedEscapedContextStoredLoggerSource);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Logger_Inside_Static_Local_Function()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            StaticLocalFunctionStoredLoggerSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Context_Is_Shadowed()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ShadowedContextStoredLoggerSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Externally_Referenced_Logger_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ExternallyReferencedStoredLoggerSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Nested_Module()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            NestedModuleLoggerSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Generic_Logger_Extension()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            LoggerWithGenericExtensionSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_When_Extension_Overload_Changes()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            LoggerWithMoreSpecificExtensionSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }

    [TestMethod]
    public async Task CodeFix_Is_Not_Offered_For_Logger_Field_With_Initializer()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            LoggerFieldWithInitializerSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }
}
