using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class V4UpgradeCodeFixTests
{
    private const string Api = """
        using System.Threading;
        using System.Threading.Tasks;
        using ModularPipelines;
        using ModularPipelines.Extensions;
        namespace ModularPipelines.TestTools
        {
            public interface IService
            {
                Task PingAsync(CancellationToken cancellationToken = default);
                Task<T> EchoAsync<T>(T value, CancellationToken cancellationToken = default);
                object Stop(CancellationToken cancellationToken = default);
            }
        }
        namespace ModularPipelines.Git
        {
            public interface IGit { IGitCommands Commands { get; } }
            public interface IGitCommands
            {
                IRepository Repository { get; }
                IHistory History { get; }
            }
            public interface IRepository
            {
                Task ConfigAsync(string options, CancellationToken cancellationToken = default);
                Task SharedAsync();
            }
            public interface IHistory { Task SharedAsync(); }
        }
        namespace ModularPipelines.Extensions
        {
            public static class ToolExtensions
            {
                extension(global::ModularPipelines.Context.IToolsContext tools)
                {
                    public global::ModularPipelines.TestTools.IService Service => throw new System.NotImplementedException();
                    public global::ModularPipelines.Git.IGit Git => throw new System.NotImplementedException();
                }
            }
        }
        """;

    [TestMethod]
    [DataRow("context.Service()", "context.Tools.Service")]
    [DataRow("context.Tools.Service.Ping()", "context.Tools.Service.PingAsync()")]
    [DataRow("context.Tools.Service.Ping(token: ct)", "context.Tools.Service.PingAsync(cancellationToken: ct)")]
    [DataRow("context.Tools.Service.Stop(token: ct)", "context.Tools.Service.Stop(cancellationToken: ct)")]
    [DataRow("context.Tools.Service.Echo<string>(\"hello\", token: ct)", "context.Tools.Service.EchoAsync<string>(\"hello\", cancellationToken: ct)")]
    [DataRow("context.Git().Commands.Config(\"option\", token: ct)", "context.Tools.Git.Commands.Repository.ConfigAsync(\"option\", cancellationToken: ct)")]
    [DataRow("context.Tools.Git.Commands.ConfigAsync(\"option\", cancellationToken: ct)", "context.Tools.Git.Commands.Repository.ConfigAsync(\"option\", cancellationToken: ct)")]
    public async Task Upgrades_Only_When_Replacement_Binds(string original, string expected)
    {
        var source = Api + $$"""
            public class Example
            {
                public object Run(IModuleContext context, CancellationToken ct) => {{original}};
            }
            """;
        var (result, changes, errors) = await ApplyFixesAsync(source);
        Assert.IsGreaterThan(0, changes, string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
        Assert.HasCount(0, errors, string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
        var expression = result.DescendantNodes().OfType<MethodDeclarationSyntax>().Single(method => method.Identifier.ValueText == "Run").ExpressionBody!.Expression;
        Assert.AreEqual(expected, expression.ToString());
    }

    [TestMethod]
    [DataRow("NotYetStarted", "NotStarted")]
    [DataRow("Processing", "Running")]
    [DataRow("Successful", "Succeeded")]
    [DataRow("UsedHistory", "RestoredFromHistory")]
    [DataRow("IgnoredFailure", "FailureIgnored")]
    [DataRow("PipelineTerminated", "Canceled")]
    [DataRow("Cancelled", "Canceled")]
    [DataRow("CachedResult", "RestoredFromCache")]
    public async Task Renames_Legacy_Status(string original, string expected)
    {
        var (root, changes, errors) = await ApplyFixesAsync($"using Status = ModularPipelines.ModuleStatus; class Example {{ Status Value => Status.{original}; }}");
        Assert.AreEqual(1, changes);
        Assert.HasCount(0, errors);
        Assert.Contains($"Status.{expected}", root.ToString());
    }

    [TestMethod]
    [DataRow("context.Tools.Git.Commands.Shared()")]
    [DataRow("context.Tools.Service.Echo(123, 456)")]
    [DataRow("context.MissingTool()")]
    [DataRow("context.Tools.Service.PingAsync()")]
    public async Task Leaves_Ambiguous_Invalid_And_Current_Calls_Unchanged(string expression)
    {
        var (_, changes, _) = await ApplyFixesAsync(Api + $$"""
            class Example { object Run(IModuleContext context) => {{expression}}; }
            """);
        Assert.AreEqual(0, changes);
    }

    [TestMethod]
    public async Task Preserves_Comments_When_Removing_Invocation_Parentheses()
    {
        var (root, changes, errors) = await ApplyFixesAsync(Api + """
            class Example { object Run(IModuleContext context) => context.Service(/* keep this */); }
            """);
        Assert.AreEqual(1, changes);
        Assert.HasCount(0, errors);
        Assert.Contains("/* keep this */", root.ToFullString());
    }

    [TestMethod]
    public async Task Leaves_Unrelated_Apis_Unchanged()
    {
        var (_, changes, _) = await ApplyFixesAsync("""
            class Service { public void PingAsync(System.Threading.CancellationToken cancellationToken = default) {} }
            enum ModuleStatus { Succeeded }
            class Example
            {
                void Run(Service service) => service.Ping(token: default);
                ModuleStatus Status => ModuleStatus.Successful;
            }
            """);
        Assert.AreEqual(0, changes);
    }

    [TestMethod]
    public async Task Fix_All_Upgrades_Independent_Occurrences()
    {
        using var workspace = new AdhocWorkspace();
        var document = CreateDocument(workspace, """
            using ModularPipelines;
            class Example
            {
                ModuleStatus First => ModuleStatus.Successful;
                ModuleStatus Second => ModuleStatus.Processing;
            }
            """);
        var provider = new V4UpgradeCodeFixProvider();
        var context = new FixAllContext(document, provider, FixAllScope.Document,
            nameof(V4UpgradeCodeFixProvider), provider.FixableDiagnosticIds,
            new CompilerDiagnosticProvider(), CancellationToken.None);
        var action = await provider.GetFixAllProvider().GetFixAsync(context);
        Assert.IsNotNull(action);
        var operations = await action.GetOperationsAsync(CancellationToken.None);
        var changed = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution.GetDocument(document.Id)!;
        var compilation = (await changed.Project.GetCompilationAsync())!;
        Assert.IsFalse(compilation.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        var text = (await changed.GetTextAsync()).ToString();
        Assert.Contains("ModuleStatus.Succeeded", text);
        Assert.Contains("ModuleStatus.Running", text);
    }

    private sealed class CompilerDiagnosticProvider : FixAllContext.DiagnosticProvider
    {
        public override async Task<IEnumerable<Diagnostic>> GetDocumentDiagnosticsAsync(Document document, CancellationToken cancellationToken)
        {
            var model = (await document.GetSemanticModelAsync(cancellationToken))!;
            return model.GetDiagnostics(cancellationToken: cancellationToken);
        }

        public override Task<IEnumerable<Diagnostic>> GetProjectDiagnosticsAsync(Project project, CancellationToken cancellationToken) =>
            Task.FromResult(Enumerable.Empty<Diagnostic>());

        public override async Task<IEnumerable<Diagnostic>> GetAllDiagnosticsAsync(Project project, CancellationToken cancellationToken) =>
            (await project.GetCompilationAsync(cancellationToken))!.GetDiagnostics(cancellationToken);
    }

    private static async Task<(SyntaxNode Root, int Changes, Diagnostic[] Errors)> ApplyFixesAsync(string source)
    {
        using var workspace = new AdhocWorkspace();
        var document = CreateDocument(workspace, source);
        return await ApplyFixesAsync(document);
    }

    private static Document CreateDocument(AdhocWorkspace workspace, string source)
    {
        var project = workspace.AddProject(ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(),
            "UpgradeTests", "UpgradeTests", LanguageNames.CSharp,
            parseOptions: new CSharpParseOptions(LanguageVersion.Preview),
            compilationOptions: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)));
        var paths = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator).Append(typeof(IModuleContext).Assembly.Location).Distinct(StringComparer.OrdinalIgnoreCase);
        project = project.AddMetadataReferences(paths.Select(path => MetadataReference.CreateFromFile(path)));
        return project.AddDocument("Example.cs", source);
    }

    private static async Task<(SyntaxNode Root, int Changes, Diagnostic[] Errors)> ApplyFixesAsync(Document document)
    {
        var provider = new V4UpgradeCodeFixProvider();
        for (var count = 0; count < 10; count++)
        {
            var compilation = (await document.Project.GetCompilationAsync())!;
            var errors = compilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();
            CodeAction? fix = null;
            foreach (var diagnostic in errors.Where(diagnostic => provider.FixableDiagnosticIds.Contains(diagnostic.Id)))
            {
                var actions = new List<CodeAction>();
                await provider.RegisterCodeFixesAsync(new CodeFixContext(document, diagnostic,
                    (action, _) => actions.Add(action), CancellationToken.None));
                Assert.IsLessThanOrEqualTo(1, actions.Count);
                if (actions.Count == 1)
                {
                    fix = actions[0];
                    break;
                }
            }

            if (fix is null)
            {
                return ((await document.GetSyntaxRootAsync())!, count, errors);
            }

            var operations = await fix.GetOperationsAsync(CancellationToken.None);
            document = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution.GetDocument(document.Id)!;
        }

        Assert.Fail("Migration did not converge within ten code fixes.");
        throw new InvalidOperationException();
    }
}
