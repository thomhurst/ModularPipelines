using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "vpc-peerings", "operations", "wait")]
public record GcloudServicesVpcPeeringsOperationsWaitOptions(
[property: CliOption("--name")] string Name
) : GcloudOptions;