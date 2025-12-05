using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "links", "describe")]
public record GcloudLoggingLinksDescribeOptions(
[property: CliArgument] string LinkId,
[property: CliOption("--bucket")] string Bucket,
[property: CliOption("--location")] string Location
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