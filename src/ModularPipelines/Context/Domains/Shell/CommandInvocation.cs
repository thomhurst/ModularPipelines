using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context.Domains.Shell;

/// <summary>
/// Describes a command after its tool and arguments have been parsed, but before execution.
/// </summary>
/// <param name="CommandLine">The parsed command line.</param>
/// <param name="ToolOptions">The original strongly typed tool options, when available.</param>
/// <param name="ExecutionOptions">The command execution options.</param>
/// <param name="CommandInput">The effective command input.</param>
/// <param name="WorkingDirectory">The effective working directory.</param>
/// <param name="EnvironmentVariables">The effective public environment variables.</param>
public sealed record CommandInvocation(
    CommandLine CommandLine,
    CommandLineToolOptions? ToolOptions,
    CommandExecutionOptions ExecutionOptions,
    string CommandInput,
    string WorkingDirectory,
    IReadOnlyDictionary<string, string?> EnvironmentVariables);
