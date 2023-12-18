using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "enrollment-group", "verify-certificate")]
public record AzIotCentralEnrollmentGroupVerifyCertificateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--group-id")] string GroupId
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--scp")]
    public string? Scp { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}

