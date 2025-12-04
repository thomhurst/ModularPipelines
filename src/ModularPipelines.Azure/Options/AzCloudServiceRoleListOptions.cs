using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud-service", "role", "list")]
public record AzCloudServiceRoleListOptions(
[property: CliOption("--cloud-service-name")] string CloudServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;