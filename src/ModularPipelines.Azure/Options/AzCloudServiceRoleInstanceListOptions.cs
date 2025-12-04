using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service", "role-instance", "list")]
public record AzCloudServiceRoleInstanceListOptions(
[property: CliOption("--cloud-service-name")] string CloudServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;