using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("offure", "vmware", "vcenter", "list")]
public record AzOffazureVmwareVcenterListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--site-name")] string SiteName
) : AzOptions;