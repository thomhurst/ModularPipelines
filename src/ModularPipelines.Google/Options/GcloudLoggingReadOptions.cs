using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "read")]
public record GcloudLoggingReadOptions(
[property: CliArgument] string LogFilter
) : GcloudOptions
{
    [CliOption("--freshness")]
    public string? Freshness { get; set; }

    [CliOption("--order")]
    public string? Order { get; set; }

    [CliOption("--billing-account")]
    public string? BillingAccount { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }

    [CliOption("--resource-names")]
    public string[]? ResourceNames { get; set; }

    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--view")]
    public string? View { get; set; }
}