using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "copy")]
public record GcloudLoggingCopyOptions(
[property: CliArgument] string BucketId,
[property: CliArgument] string Destination,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--log-filter")]
    public string? LogFilter { get; set; }

    [CliOption("--billing-account")]
    public string? BillingAccount { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}