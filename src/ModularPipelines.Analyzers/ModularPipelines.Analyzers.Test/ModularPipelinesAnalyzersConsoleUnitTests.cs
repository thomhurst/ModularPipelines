using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ConsoleUseAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModularPipelinesAnalyzersConsoleUnitTests
{
    private const string BadModuleSource = @"
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

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);

        {|#0:Console.WriteLine(""Done!"")|};

        return new List<string>();
    }
}
";
    
    private const string BadModuleSource2 = @"
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

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);

        {|#0:Console.Write(""Done!"")|};

        return new List<string>();
    }
}
";
    
    private const string BadModuleSource3 = @"
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

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);

        {|#0:Console.Out.Write(""Done!"")|};

        return new List<string>();
    }
}
";
    
    private const string BadModuleSource4 = @"
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

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);

        {|#0:Console.Out.WriteLine(""Done!"")|};

        return new List<string>();
    }
}
";
    
    private const string BadModuleSource5 = @"
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

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);

        await {|#0:Console.Out.WriteLineAsync(""Done!"")|};

        return new List<string>();
    }
}
";
    
    private const string BadModuleSource6 = @"
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

namespace ModularPipelines.Examples.Modules;

public class Module1 : Module<List<string>>
{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);

        {|#0:Console.Out.Dispose()|};

        return new List<string>();
    }
}
";
    
    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource, expected);
    }
    
    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console2()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource2, expected);
    }
    
    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console3()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource3, expected);
    }
    
    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console4()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource4, expected);
    }
    
    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console5()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);

        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource5, expected);
    }
    
    [TestMethod]
    public async Task AnalyzerIsTriggered_When_Using_Console6()
    {
        var expected = VerifyCS.Diagnostic(ConsoleUseAnalyzer.DiagnosticId).WithLocation(0);
        
        await VerifyCS.VerifyAnalyzerAsync(BadModuleSource6, expected);
    }
}
