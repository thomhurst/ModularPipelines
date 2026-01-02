using Microsoft.CodeAnalysis.Testing;

using VerifyCS = ModularPipelines.Development.Analyzers.UnitTests.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Development.Analyzers.VirtualCommandAnalyzer,
    ModularPipelines.Development.Analyzers.VirtualCommandCodeFixProvider>;

namespace ModularPipelines.Development.Analyzers.UnitTests;

public class VirtualCommandAnalyzerTests
{
    private const string TestSource = @"
using System.Threading.Tasks;

namespace ModularPipelines.Models
{
    public class CommandResult
    {
        public int ExitCode { get; set; }
        public string StandardOutput { get; set; } = string.Empty;
        public string StandardError { get; set; } = string.Empty;
    }
}

namespace TestNamespace
{
    using ModularPipelines.Models;

    <<CLASS>>
}
";

    private static string CreateSource(string classCode)
    {
        return TestSource.Replace("<<CLASS>>", classCode);
    }

    [Test]
    public async Task AnalyzerIsTriggered_When_NonVirtual_CommandResultTask_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        public Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        // Line 21 is where the method declaration starts
        var expected = VerifyCS.Diagnostic(VirtualCommandAnalyzer.DiagnosticId).WithSpan(21, 9, 24, 10);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Virtual_CommandResultTask_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        public virtual Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Private_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        private Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Static_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        public static Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Abstract_Method()
    {
        var source = CreateSource(@"
    public abstract class TestService
    {
        public abstract Task<CommandResult> SomeMethod();
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Override_Method()
    {
        var source = CreateSource(@"
    public class BaseService
    {
        public virtual Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }

    public class TestService : BaseService
    {
        public override Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Sealed_Class()
    {
        var source = CreateSource(@"
    public sealed class TestService
    {
        public Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Sealed_Method()
    {
        var source = CreateSource(@"
    public class BaseService
    {
        public virtual Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }

    public class TestService : BaseService
    {
        public sealed override Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Different_Return_Type()
    {
        var source = @"
using System.Threading.Tasks;

namespace TestNamespace
{
    public class TestService
    {
        public Task<string> SomeMethod()
        {
            return Task.FromResult(""test"");
        }
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsTriggered_For_Protected_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        protected Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualCommandAnalyzer.DiagnosticId).WithSpan(21, 9, 24, 10);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [Test]
    public async Task AnalyzerIsTriggered_For_Internal_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        internal Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualCommandAnalyzer.DiagnosticId).WithSpan(21, 9, 24, 10);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [Test]
    public async Task CodeFix_Adds_Virtual_Keyword_To_Public_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        public Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        var fixedSource = CreateSource(@"
    public class TestService
    {
        public virtual Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualCommandAnalyzer.DiagnosticId).WithSpan(21, 9, 24, 10);
        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Test]
    public async Task CodeFix_Adds_Virtual_Keyword_To_Protected_Method()
    {
        var source = CreateSource(@"
    public class TestService
    {
        protected Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        var fixedSource = CreateSource(@"
    public class TestService
    {
        protected virtual Task<CommandResult> SomeMethod()
        {
            return Task.FromResult(new CommandResult());
        }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualCommandAnalyzer.DiagnosticId).WithSpan(21, 9, 24, 10);
        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Test]
    public async Task CodeFix_Adds_Virtual_Keyword_After_Async_Modifier()
    {
        var source = CreateSource(@"
    public class TestService
    {
        public async Task<CommandResult> SomeMethod()
        {
            return await Task.FromResult(new CommandResult());
        }
    }
");

        var fixedSource = CreateSource(@"
    public class TestService
    {
        public virtual async Task<CommandResult> SomeMethod()
        {
            return await Task.FromResult(new CommandResult());
        }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualCommandAnalyzer.DiagnosticId).WithSpan(21, 9, 24, 10);
        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}
