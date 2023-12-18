using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("offure", "hyperv", "host", "list")]
public record AzOffazureHypervHostListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--site-name")] string SiteName
) : AzOptions;