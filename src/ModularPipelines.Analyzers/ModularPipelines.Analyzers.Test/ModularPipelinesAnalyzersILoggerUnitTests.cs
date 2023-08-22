using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.LoggerInConstructorAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersILoggerUnitTests
{
    private const string BadModuleSourceILogger = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    public Module1({|#0:ILogger logger|})
    {
    }

    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}
";
    
    private const string BadModuleSourceILoggerProvider = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    public Module1({|#0:ILoggerProvider loggerProvider|})
    {
    }

    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}
";
    
    private const string BadModuleSourceILoggerFactory = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    public Module1({|#0:ILoggerFactory loggerFactory|})
    {
    }

    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}
";
    
    private const string BadModuleSourceILoggerGeneric = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    public Module1({|#0:ILogger<Module1> logger|})
    {
    }

    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}
";
    
    private const string GoodModuleSource = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}
";
    
    private const string GoodModuleSource2 = @"
#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    public Module1(IModuleLoggerProvider moduleLoggerProvider)
    {
    }

    protected override async Task<ModuleResult<List<string>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }
}
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
