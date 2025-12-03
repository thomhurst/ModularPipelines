using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "resources", "list")]
public record GcloudInfraManagerResourcesListOptions(
[property: CliOption("--revision")] string Revision,
[property: CliOption("--deployment")] string Deployment,
[property: CliOption("--location")] string Location
) : GcloudOptions;