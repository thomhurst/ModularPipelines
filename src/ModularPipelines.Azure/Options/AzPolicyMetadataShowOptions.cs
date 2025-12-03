using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "metadata", "show")]
public record AzPolicyMetadataShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;