using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "private-cloud", "identity", "remove")]
public record AzVmwarePrivateCloudIdentityRemoveOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--system-assigned")]
    public bool? SystemAssigned { get; set; }
}