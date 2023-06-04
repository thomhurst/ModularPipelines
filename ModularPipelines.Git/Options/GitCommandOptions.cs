using ModularPipelines.Command.Options;

namespace ModularPipelines.Git.Options;

public record GitCommandOptions(IEnumerable<string> Arguments) : CommandEnvironmentOptions
{
}