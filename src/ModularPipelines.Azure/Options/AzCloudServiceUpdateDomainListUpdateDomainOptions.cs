using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud-service", "update-domain", "list-update-domain")]
public record AzCloudServiceUpdateDomainListUpdateDomainOptions(
[property: CommandSwitch("--cloud-service-name")] string CloudServiceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;