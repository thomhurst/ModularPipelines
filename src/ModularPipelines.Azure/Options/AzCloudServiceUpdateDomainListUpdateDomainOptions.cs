using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service", "update-domain", "list-update-domain")]
public record AzCloudServiceUpdateDomainListUpdateDomainOptions(
[property: CliOption("--cloud-service-name")] string CloudServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;