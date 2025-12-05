using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "troubleshoot", "command", "scvmm")]
public record AzArcapplianceTroubleshootCommandScvmmOptions : AzOptions
{
    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }
}