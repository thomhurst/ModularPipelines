using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "enrollment-group", "show")]
public record AzIotCentralEnrollmentGroupShowOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--group-id")] string GroupId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--certificate-entry")]
    public string? CertificateEntry { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}