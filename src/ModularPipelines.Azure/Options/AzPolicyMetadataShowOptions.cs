using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "metadata", "show")]
public record AzPolicyMetadataShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}