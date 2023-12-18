using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("request-pull")]
[ExcludeFromCodeCoverage]
public record GitRequestPullOptions : GitOptions;