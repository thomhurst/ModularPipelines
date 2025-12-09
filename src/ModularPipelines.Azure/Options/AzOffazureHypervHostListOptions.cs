using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("offure", "hyperv", "host", "list")]
public record AzOffazureHypervHostListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--site-name")] string SiteName
) : AzOptions;