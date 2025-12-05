using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "sinks", "delete")]
public record GcloudLoggingSinksDeleteOptions(
[property: CliArgument] string SinkName
) : GcloudOptions
{
    [CliOption("--billing-account")]
    public string? BillingAccount { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}