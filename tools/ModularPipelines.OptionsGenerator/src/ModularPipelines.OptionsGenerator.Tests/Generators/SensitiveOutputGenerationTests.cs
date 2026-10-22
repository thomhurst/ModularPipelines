using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class SensitiveOutputGenerationTests
{
    [Test]
    [Arguments(false, 0)]
    [Arguments(false, 1)]
    [Arguments(false, 2)]
    [Arguments(true, 0)]
    [Arguments(true, 1)]
    [Arguments(true, 2)]
    public async Task Generated_Method_Preserves_Result_And_Caller_Settings(bool suppressOutput, int scenario)
    {
        var command = new CliCommandDefinition
        {
            FullCommand = "tool print token",
            CommandParts = ["print", "token"],
            ClassName = "TokenOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [],
            SuppressOutputLoggingByDefault = suppressOutput,
        };
        var method = new StringBuilder();
        GeneratorUtils.GenerateServiceMethod(method, "PrintToken", command);
        var source = $$"""
            using System;
            using System.Threading;
            using System.Threading.Tasks;
            public enum CommandLogVerbosity { Silent, Normal }
            public record CommandLoggingOptions
            {
                public CommandLogVerbosity Verbosity { get; init; } = CommandLogVerbosity.Normal;
                public static CommandLoggingOptions Silent { get; } = new() { Verbosity = CommandLogVerbosity.Silent };
                public bool IncludesStandardOutput => ShowStandardOutput ?? Verbosity >= CommandLogVerbosity.Normal;
                public bool IncludesStandardError => ShowStandardError ?? Verbosity >= CommandLogVerbosity.Normal;
                public bool? ShowStandardOutput { get; init; }
                public bool? ShowStandardError { get; init; }
            }
            public record CommandExecutionOptions
            {
                public CommandLoggingOptions? Logging { get; init; }
                public string? WorkingDirectory { get; init; }
                public TimeSpan? ExecutionTimeout { get; init; }
            }
            public record TokenOptions;
            public record CommandResult(string StandardOutput);
            public class Context
            {
                public CommandExecutionOptions? Received;
                public CancellationToken Cancellation;
                public Task<CommandResult> ExecuteCommandLineToolAsync(TokenOptions options,
                    CommandExecutionOptions? executionOptions, CancellationToken cancellationToken)
                {
                    if (options is null) throw new Exception("Missing command options");
                    Received = executionOptions;
                    Cancellation = cancellationToken;
                    return Task.FromResult(new CommandResult("token-helper-value"));
                }
            }
            public class Service
            {
                private readonly Context _command;
                public Service(Context command) => _command = command;
                {{method}}
            }
            public static class Consumer
            {
                public static async Task<bool> Run(bool suppressOutput, int scenario)
                {
                    var context = new Context();
                    var logging = new CommandLoggingOptions { ShowStandardOutput = true, ShowStandardError = true };
                    var execution = scenario == 0 ? null : new CommandExecutionOptions
                    {
                        WorkingDirectory = "work",
                        ExecutionTimeout = TimeSpan.FromSeconds(17),
                        Logging = scenario == 2 ? logging : null,
                    };
                    using var cancellation = new CancellationTokenSource();
                    var result = await new Service(context).PrintTokenAsync(cancellationToken: cancellation.Token,
                        executionOptions: execution);
                    if (result.StandardOutput != "token-helper-value" || context.Cancellation != cancellation.Token) return false;
                    if (scenario > 0 && (context.Received?.WorkingDirectory != "work"
                        || context.Received.ExecutionTimeout != TimeSpan.FromSeconds(17))) return false;
                    if (!suppressOutput || scenario == 2) return ReferenceEquals(context.Received, execution);
                    return context.Received?.Logging?.IncludesStandardOutput == false
                        && context.Received.Logging.IncludesStandardError == false
                        && context.Received.Logging.Verbosity == CommandLogVerbosity.Silent
                        && execution?.Logging is null;
                }
            }
            """;
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(Guid.NewGuid().ToString("N"),
            [CSharpSyntaxTree.ParseText(source)], references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        if (!result.Success)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
        }

        var assembly = System.Reflection.Assembly.Load(stream.ToArray());
        var success = await (Task<bool>) assembly.GetType("Consumer")!.GetMethod("Run")!
            .Invoke(null, [suppressOutput, scenario])!;
        await Assert.That(success).IsTrue();
    }
}
