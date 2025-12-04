using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "custom-domain", "show")]
public record AzSpringAppCustomDomainShowOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions;