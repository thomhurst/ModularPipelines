#nullable enable

using Microsoft.CodeAnalysis.Testing;

using VerifyCS = ModularPipelines.Development.Analyzers.UnitTests.Verifiers.CSharpCodeFixVerifier<
    ModularPipelines.Development.Analyzers.VirtualSwitchPropertyAnalyzer,
    ModularPipelines.Development.Analyzers.VirtualSwitchPropertyCodeFixProvider>;

namespace ModularPipelines.Development.Analyzers.UnitTests;

public class VirtualSwitchPropertyAnalyzerTests
{
    private const string TestSource = @"
#nullable enable

namespace ModularPipelines.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class CliOptionAttribute : System.Attribute
    {
        public CliOptionAttribute(string name) { }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class CliFlagAttribute : System.Attribute
    {
        public CliFlagAttribute(string name) { }
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class CliArgumentAttribute : System.Attribute
    {
        public CliArgumentAttribute(int position) { }
    }
}

namespace TestNamespace
{
    using ModularPipelines.Attributes;

    <<CLASS>>
}
";

    private static string CreateSource(string classCode)
    {
        return TestSource.Replace("<<CLASS>>", classCode);
    }

    [Test]
    public async Task AnalyzerIsTriggered_When_NonVirtual_CliOption_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliOption(""--test"")]
        public string? TestOption { get; set; }
    }
");

        // Line 32 is where the attribute starts, line 33 is property
        var expected = VerifyCS.Diagnostic(VirtualSwitchPropertyAnalyzer.DiagnosticId).WithSpan(32, 9, 33, 48);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [Test]
    public async Task AnalyzerIsTriggered_When_NonVirtual_CliFlag_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliFlag(""--verbose"")]
        public bool Verbose { get; set; }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualSwitchPropertyAnalyzer.DiagnosticId).WithSpan(32, 9, 33, 42);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [Test]
    public async Task AnalyzerIsTriggered_When_NonVirtual_CliArgument_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliArgument(0)]
        public string? FilePath { get; set; }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualSwitchPropertyAnalyzer.DiagnosticId).WithSpan(32, 9, 33, 46);
        await VerifyCS.VerifyAnalyzerAsync(source, expected);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Virtual_CliOption_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliOption(""--test"")]
        public virtual string? TestOption { get; set; }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Virtual_CliFlag_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliFlag(""--verbose"")]
        public virtual bool Verbose { get; set; }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Virtual_CliArgument_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliArgument(0)]
        public virtual string? FilePath { get; set; }
    }
");

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsNotTriggered_When_Property_Has_No_Cli_Attribute()
    {
        var source = @"
#nullable enable

namespace TestNamespace
{
    public class TestOptions
    {
        public string? TestOption { get; set; }
    }
}";

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [Test]
    public async Task AnalyzerIsTriggered_Multiple_NonVirtual_Properties()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliOption(""--name"")]
        public string? Name { get; set; }

        [CliFlag(""--verbose"")]
        public bool Verbose { get; set; }
    }
");

        var expected1 = VerifyCS.Diagnostic(VirtualSwitchPropertyAnalyzer.DiagnosticId).WithSpan(32, 9, 33, 42);
        var expected2 = VerifyCS.Diagnostic(VirtualSwitchPropertyAnalyzer.DiagnosticId).WithSpan(35, 9, 36, 42);
        await VerifyCS.VerifyAnalyzerAsync(source, expected1, expected2);
    }

    [Test]
    public async Task CodeFix_Adds_Virtual_Keyword_To_CliOption_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliOption(""--test"")]
        public string? TestOption { get; set; }
    }
");

        var fixedSource = CreateSource(@"
    public class TestOptions
    {
        [CliOption(""--test"")]
        public virtual string? TestOption { get; set; }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualSwitchPropertyAnalyzer.DiagnosticId).WithSpan(32, 9, 33, 48);
        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [Test]
    public async Task CodeFix_Adds_Virtual_Keyword_To_CliFlag_Property()
    {
        var source = CreateSource(@"
    public class TestOptions
    {
        [CliFlag(""--verbose"")]
        public bool Verbose { get; set; }
    }
");

        var fixedSource = CreateSource(@"
    public class TestOptions
    {
        [CliFlag(""--verbose"")]
        public virtual bool Verbose { get; set; }
    }
");

        var expected = VerifyCS.Diagnostic(VirtualSwitchPropertyAnalyzer.DiagnosticId).WithSpan(32, 9, 33, 42);
        await VerifyCS.VerifyCodeFixAsync(source, expected, fixedSource);
    }
}
