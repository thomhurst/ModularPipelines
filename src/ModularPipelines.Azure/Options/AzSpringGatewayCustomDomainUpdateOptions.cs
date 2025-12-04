using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "gateway", "custom-domain", "update")]
public record AzSpringGatewayCustomDomainUpdateOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }
}