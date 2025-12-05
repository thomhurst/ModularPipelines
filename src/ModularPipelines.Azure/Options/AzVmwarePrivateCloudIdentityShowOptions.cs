using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "private-cloud", "identity", "show")]
public record AzVmwarePrivateCloudIdentityShowOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;