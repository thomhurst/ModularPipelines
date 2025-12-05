using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("offure", "vmware", "run-as-account", "list")]
public record AzOffazureVmwareRunAsAccountListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--site-name")] string SiteName
) : AzOptions;