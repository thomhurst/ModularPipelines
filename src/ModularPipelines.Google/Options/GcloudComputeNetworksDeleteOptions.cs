using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "delete")]
public record GcloudComputeNetworksDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;