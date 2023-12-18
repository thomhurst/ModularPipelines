using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "custom-domain", "bind")]
public record AzSpringAppCustomDomainBindOptions(
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