using ModularPipelines.Git.Enums;
using ModularPipelines.Options;

namespace ModularPipelines.Git.Options;

public record GitStageOptions(GitStageOption GitStageOption) : CommandLineOptions;
