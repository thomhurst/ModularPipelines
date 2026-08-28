using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;

namespace ModularPipelines.Testing;

/// <summary>
/// A parsed command invocation and its intercepted result.
/// </summary>
/// <param name="Invocation">The intercepted invocation.</param>
/// <param name="Result">The result returned to the module.</param>
public sealed record RecordedCommand(CommandInvocation Invocation, CommandResult Result)
{
    /// <summary>
    /// Gets the parsed command line.
    /// </summary>
    public CommandLine CommandLine => Invocation.CommandLine;
}
