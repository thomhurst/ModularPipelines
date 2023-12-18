using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "ssl", "create")]
public record AzFunctionappConfigSslCreateOptions(
[property: CommandSwitch("--hostname")] string Hostname,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--certificate-name")]
    public string? CertificateName { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}