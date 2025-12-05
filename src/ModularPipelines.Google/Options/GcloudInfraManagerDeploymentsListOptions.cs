using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "list")]
public record GcloudInfraManagerDeploymentsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;