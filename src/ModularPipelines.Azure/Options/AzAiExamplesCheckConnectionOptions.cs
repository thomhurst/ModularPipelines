using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-examples", "check-connection")]
public record AzAiExamplesCheckConnectionOptions : AzOptions;