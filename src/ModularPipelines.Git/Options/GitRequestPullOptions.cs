using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("request-pull")]
[ExcludeFromCodeCoverage]
public record GitRequestPullOptions : GitOptions;