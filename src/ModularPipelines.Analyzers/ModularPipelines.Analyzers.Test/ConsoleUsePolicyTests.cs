using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ConsoleUsePolicyTests
{
    [TestMethod]
    [DataRow("Console.Write(\"Building...\")")]
    [DataRow("Console.WriteLine(\"Build succeeded\")")]
    [DataRow("Console.Out.WriteLine(\"Build succeeded\")")]
    [DataRow("Console.Error.Write(\"Warning: \")")]
    [DataRow("Console.Error.WriteLine(\"Build failed\")")]
    [DataRow("await Console.Error.WriteLineAsync(\"Build failed\")")]
    public async Task DocumentedConsoleOutputIsAllowedByDefault(string consoleCall)
    {
        var source = $$"""
            {{TestSourceConstants.StandardUsings}}

            {{consoleCall}};
            await Task.CompletedTask;

            public class BuildModule : Module<int>
            {
                protected override async Task<int> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    {{consoleCall}};
                    await Task.CompletedTask;
                    return 0;
                }
            }
            """;

        var diagnostics = await AnalyzeAsync(source);

        Assert.AreEqual(0, diagnostics.Length);
    }

    [TestMethod]
    public void ConsolePolicyIsDisabledByDefault()
    {
        Assert.IsFalse(ConsoleUseAnalyzer.Rule.IsEnabledByDefault);
        Assert.AreEqual(DiagnosticSeverity.Warning, ConsoleUseAnalyzer.Rule.DefaultSeverity);
    }

    [TestMethod]
    public async Task ConsolePolicyCanBeEnabledExplicitly()
    {
        var diagnostics = await AnalyzeAsync(
            "System.Console.WriteLine(\"Starting pipeline\");",
            ReportDiagnostic.Warn);

        Assert.AreEqual(1, diagnostics.Length);
        Assert.AreEqual(ConsoleUseAnalyzer.DiagnosticId, diagnostics[0].Id);
        Assert.AreEqual(DiagnosticSeverity.Warning, diagnostics[0].Severity);
    }

    [TestMethod]
    public async Task ExplicitConsolePolicyRespectsSuppression()
    {
        var diagnostics = await AnalyzeAsync(
            """
            #pragma warning disable MP0004
            System.Console.Error.WriteLine("Pipeline failed");
            #pragma warning restore MP0004
            """,
            ReportDiagnostic.Warn);

        Assert.AreEqual(0, diagnostics.Length);
    }

    private static async Task<ImmutableArray<Diagnostic>> AnalyzeAsync(
        string source, ReportDiagnostic severity = ReportDiagnostic.Default)
    {
        var trustedPlatformAssemblies =
            (string?) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty;
        var referencePaths = trustedPlatformAssemblies
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Append(typeof(IModuleContext).Assembly.Location)
            .Distinct(StringComparer.OrdinalIgnoreCase);
        var options = new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            .WithSpecificDiagnosticOptions(
                ImmutableDictionary<string, ReportDiagnostic>.Empty.Add(
                    ConsoleUseAnalyzer.DiagnosticId, severity));
        var compilation = CSharpCompilation.Create(
            "ConsolePolicyTest",
            [CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview))],
            referencePaths.Select(path => MetadataReference.CreateFromFile(path)),
            options);
        var errors = compilation.GetDiagnostics()
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        Assert.AreEqual(0, errors.Length, string.Join(Environment.NewLine, errors.Select(error => error.ToString())));

        return await compilation
            .WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(new ConsoleUseAnalyzer()))
            .GetAnalyzerDiagnosticsAsync();
    }
}
