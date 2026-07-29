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

    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    private const string FixedModuleSourceILoggerGeneric = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        return new List<string>();
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

    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        await Task.Delay(1, cancellationToken);
        context.Logger.LogInformation(""Running"");
        return new List<string>();
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

    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        _logger.LogInformation(""Running"");
        return Task.FromResult<List<string>?>([]);
    }}
}}
";

    private const string FixedEscapedContextStoredLoggerSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override Task<List<string>?> ExecuteAsync(IModuleContext @event, CancellationToken cancellationToken)
    {{
        @event.Logger.LogInformation(""Running"");
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        static void Log()
        {{
            _logger.LogInformation(""Running"");
        }}

        Log();
        return Task.FromResult<List<string>?>([]);
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

    protected override Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {{
        return Task.FromResult<List<string>?>([]);
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

    private const string GoodModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    public async Task CodeFix_Is_Not_Offered_For_Externally_Referenced_Logger_Field()
    {
        await VerifyCS.VerifyNoCodeFixAsync(
            ExternallyReferencedStoredLoggerSource,
            LoggerInConstructorAnalyzer.DiagnosticId);
    }
}
