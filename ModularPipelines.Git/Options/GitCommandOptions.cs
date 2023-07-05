using ModularPipelines.Options;

namespace ModularPipelines.Git.Options;

public record GitCommandOptions(IEnumerable<string> Arguments) : CommandLineOptions;