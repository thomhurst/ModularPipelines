using System.Text;

namespace ModularPipelines.Testing;

/// <summary>
/// A parsed command invocation and its intercepted result.
/// </summary>
/// <remarks>Diagnostic printing uses the masked invocation and redacts the raw result.</remarks>
/// <param name="Invocation">The intercepted invocation.</param>
/// <param name="Result">The result returned to the module.</param>
public sealed record RecordedCommand(CommandInvocation Invocation, CommandResult Result)
{
    /// <summary>
    /// Gets the parsed command line.
    /// </summary>
    public CommandLine CommandLine => Invocation.CommandLine;

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Invocation = ").Append(Invocation);
        // Interceptors can return raw command inputs and output containing secrets.
        builder.Append(", Result = <redacted>");
        return true;
    }
}
