using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "custom-domain", "show")]
public record AzSpringAppCustomDomainShowOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [BooleanCommandSwitch("--enable-ingress-to-app-tls")]
    public bool? EnableIngressToAppTls { get; set; }
}

