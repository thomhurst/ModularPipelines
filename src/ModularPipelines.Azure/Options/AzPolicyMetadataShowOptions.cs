using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "metadata", "show")]
public record AzPolicyMetadataShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;