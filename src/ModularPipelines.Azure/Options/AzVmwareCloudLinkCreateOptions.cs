using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "cloud-link", "create")]
public record AzVmwareCloudLinkCreateOptions(
[property: CliOption("--cloud-link-name")] string CloudLinkName,
[property: CliOption("--linked-cloud")] string LinkedCloud,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}