using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.LoggerInConstructorAnalyzer>;

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

    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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

    private const string GoodModuleSource = $@"
{TestSourceConstants.StandardModuleHeaderWithLogging}

public class Module1 : Module<List<string>>
{{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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

    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
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
}