using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularPipelines.Interfaces;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleLocalFunctionCancellationAnalyzerTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_In_Returned_Lambda(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                var unrelatedToken = new CancellationToken();
                return Task.Run(() => {|#0:Task.Delay(1, {{argument}})|}, token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Captured_By_Returned_Local_Function(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync();

                Task RunAsync() => Task.Run(() => { }, token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Captured_By_Returned_Lambda(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return Task.Run(() => Task.Delay(1, token), token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Forwarded_To_Returned_Local_Function_In_Helper(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                Task RunAsync(CancellationToken localToken) => Task.Run(() => { }, localToken);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Forwarded_Through_Returned_Local_Functions(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return Outer(token);

                Task Outer(CancellationToken outerToken) => Inner(outerToken);
                Task Inner(CancellationToken innerToken) => Task.Run(() => { }, innerToken);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_In_Local_Function_In_Helper(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                async Task RunAsync(CancellationToken localToken)
                {
                    var unrelatedToken = new CancellationToken();
                    await {|#0:Task.Run(() => { }, {{argument}})|};
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_Forwarded_To_Local_Function(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                var unrelatedToken = new CancellationToken();
                return {|#0:RunAsync({{argument}})|};

                async Task RunAsync(CancellationToken localToken)
                {
                    await Task.Run(() => { }, localToken);
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("RunAsync"));
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_Between_Local_Functions(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return Outer(token);

                async Task Outer(CancellationToken outerToken)
                {
                    var unrelatedToken = new CancellationToken();
                    await {|#0:Inner({{argument}})|};
                }

                async Task Inner(CancellationToken innerToken)
                {
                    await Task.Run(() => { }, innerToken);
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Inner"));
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Forwarded_Through_Local_Wrapper_In_Entry(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                Task RunAsync(CancellationToken localToken) => Task.Run(() => { }, localToken);
            }
            """);
        var original = source;
        source = source.Replace("=> HelperAsync(cancellationToken);", "{ return Wrapper(); Task Wrapper() => HelperAsync(cancellationToken); }")
            .Replace("await HelperAsync(cancellationToken); return false;", "await Wrapper(); return false; Task Wrapper() => HelperAsync(cancellationToken);");

        Assert.AreNotEqual(original, source);
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", false)]
    [DataRow("lifecycle", false)]
    [DataRow("configuration", false)]
    [DataRow("execute", true)]
    [DataRow("lifecycle", true)]
    [DataRow("configuration", true)]
    public async Task Reports_Once_Per_Returned_Local_Function_Invocation(string root, bool chained)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return {{(chained ? "ForwardAsync(token)" : "RunAsync(token)")}};
                {{(chained ? "Task ForwardAsync(CancellationToken token) => RunAsync(token);" : "")}}
                Task RunAsync(CancellationToken localToken) => Task.WhenAll(
                    {|#0:Task.Run(() => { }, CancellationToken.None)|},
                    {|#1:Task.Delay(1, CancellationToken.None)|},
                    Task.Delay(1, localToken));
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Run"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("execute", false)]
    [DataRow("lifecycle", false)]
    [DataRow("configuration", false)]
    [DataRow("execute", true)]
    [DataRow("lifecycle", true)]
    [DataRow("configuration", true)]
    public async Task Preserves_Local_Semantic_Diagnostics(string root, bool chained)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return {{(chained ? "ForwardAsync(token)" : "RunAsync(token)")}};
                {{(chained ? "Task ForwardAsync(CancellationToken token) => RunAsync(token);" : "")}}
                Task RunAsync(CancellationToken localToken) => Task.WhenAll(
                    Task.Run(() => { }, CancellationToken.None),
                    Task.Delay(1, CancellationToken.None),
                    Task.Delay(1, localToken));
            }
            """);
        var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview), cancellationToken: TestContext.CancellationToken);
        var referencePaths = ((string?) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Append(typeof(IModuleContext).Assembly.Location)
            .Distinct(StringComparer.OrdinalIgnoreCase);
        var compilation = CSharpCompilation.Create(
            "LocalSemanticDiagnostics", [tree],
            referencePaths.Select(path => MetadataReference.CreateFromFile(path)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var errors = compilation.GetDiagnostics(TestContext.CancellationToken).Where(d => d.Severity == DiagnosticSeverity.Error).ToArray();
        Assert.IsEmpty(errors, $"{string.Join(Environment.NewLine, errors.Select(d => d.ToString()))}");

        var analysis = compilation.WithAnalyzers(
            [new ModuleAsyncSafetyAnalyzer()],
            new CompilationWithAnalyzersOptions(
                options: new AnalyzerOptions([]), onAnalyzerException: null, concurrentAnalysis: true, logAnalyzerExecutionTime: false));
        var diagnostics = await analysis.GetAnalyzerSemanticDiagnosticsAsync(
            compilation.GetSemanticModel(tree), null, TestContext.CancellationToken);
        var warnings = diagnostics.Where(d => d.Id == ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).ToArray();
        Assert.HasCount(2, warnings, $"{string.Join(Environment.NewLine, diagnostics.Select(d => d.ToString()))}");
        var diagnosticSources = warnings
            .Select(d => source.Substring(d.Location.SourceSpan.Start, d.Location.SourceSpan.Length)).ToArray();
        Assert.Contains("Task.Run(() => { }, CancellationToken.None)", diagnosticSources);
        Assert.Contains("Task.Delay(1, CancellationToken.None)", diagnosticSources);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Valid_Handler_Rebinding_In_Returned_Callback(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { Throw(); }
                catch { alias = ref other; alias = default; }
                return Task.Run(() => Task.Delay(1, token), token);
            }

            private static void Throw() => throw new InvalidOperationException();
            """);
        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Unchanged_Saved_Token_Restored_In_Handler(string root)
    {
        string[] declarations = ["var original = token;", "var saved = token; var original = saved;"];
        foreach (var declaration in declarations)
        {
            var source = ModuleSource(root, $$"""
                private static Task HelperAsync(CancellationToken token)
                {
                    {{declaration}}
                    try { Throw(); }
                    catch { token = original; }
                    return Task.Run(() => Task.Delay(1, token), token);
                }

                private static void Throw() => throw new System.InvalidOperationException();
                """);

            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }

    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Does_Not_Report_Token_Restored_From_Saved_Copy_In_Callback(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                var original = token;
                return Task.Run(() =>
                {
                    try { Throw(); }
                    catch { token = original; }
                    return Task.Delay(1, token);
                }, token);
            }

            private static void Throw() => throw new System.InvalidOperationException();
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    private static string ModuleSource(string root, string helper)
    {
        var entry = root switch
        {
            "execute" => "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            "lifecycle" => "protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            _ => "protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(async (context, cancellationToken) => { await HelperAsync(cancellationToken); return false; });",
        };

        return $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public sealed class BuildModule : Module
            {
                {{(root == "execute" ? "" : "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;")}}
                {{entry}}

                {{helper}}
            }
            """;
    }
}
