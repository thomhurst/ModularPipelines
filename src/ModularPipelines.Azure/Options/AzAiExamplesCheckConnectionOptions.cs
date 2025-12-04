using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ai-examples", "check-connection")]
public record AzAiExamplesCheckConnectionOptions : AzOptions;