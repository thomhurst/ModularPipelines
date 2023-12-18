using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service", "role", "list")]
public record AzCloudServiceRoleListOptions(
[property: CommandSwitch("--cloud-service-name")] string CloudServiceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;