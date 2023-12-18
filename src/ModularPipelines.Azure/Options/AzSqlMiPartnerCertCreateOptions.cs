using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "partner-cert", "create")]
public record AzSqlMiPartnerCertCreateOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--instance-name")] string InstanceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-blob")]
    public string? PublicBlob { get; set; }
}

