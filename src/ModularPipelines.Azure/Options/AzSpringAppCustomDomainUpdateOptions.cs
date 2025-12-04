using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "custom-domain", "update")]
public record AzSpringAppCustomDomainUpdateOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliFlag("--enable-ingress-to-app-tls")]
    public bool? EnableIngressToAppTls { get; set; }
}