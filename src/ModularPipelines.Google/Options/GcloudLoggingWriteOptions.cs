using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "write")]
public record GcloudLoggingWriteOptions(
[property: CliArgument] string LogName,
[property: CliArgument] string Message
) : GcloudOptions
{
    [CliOption("--payload-type")]
    public string? PayloadType { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--billing-account")]
    public string? BillingAccount { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}