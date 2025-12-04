using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "troubleshoot", "command", "hci")]
public record AzArcapplianceTroubleshootCommandHciOptions : AzOptions
{
    [CliOption("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }
}