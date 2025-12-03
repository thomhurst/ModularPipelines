using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("request-pull")]
[ExcludeFromCodeCoverage]
public record GitRequestPullOptions : GitOptions;