using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "custom-domain", "list")]
public record AzSpringAppCustomDomainListOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions;