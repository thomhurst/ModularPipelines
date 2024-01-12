using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "resources", "list")]
public record GcloudInfraManagerResourcesListOptions(
[property: CommandSwitch("--revision")] string Revision,
[property: CommandSwitch("--deployment")] string Deployment,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;