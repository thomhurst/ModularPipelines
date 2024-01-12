using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "list")]
public record GcloudInfraManagerDeploymentsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;