using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "troubleshoot", "command", "vmware")]
public record AzArcapplianceTroubleshootCommandVmwareOptions : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}