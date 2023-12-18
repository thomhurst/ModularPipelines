using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "certificate", "create")]
public record AzContainerappEnvCertificateCreateOptions(
[property: CommandSwitch("--hostname")] string Hostname,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--validation-method")] string ValidationMethod
) : AzOptions
{
    [CommandSwitch("--certificate-name")]
    public string? CertificateName { get; set; }
}