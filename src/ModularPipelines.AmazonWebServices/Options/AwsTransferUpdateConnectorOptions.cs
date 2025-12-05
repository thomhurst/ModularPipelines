using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "update-connector")]
public record AwsTransferUpdateConnectorOptions(
[property: CliOption("--connector-id")] string ConnectorId
) : AwsOptions
{
    [CliOption("--url")]
    public string? Url { get; set; }

    [CliOption("--as2-config")]
    public string? As2Config { get; set; }

    [CliOption("--access-role")]
    public string? AccessRole { get; set; }

    [CliOption("--logging-role")]
    public string? LoggingRole { get; set; }

    [CliOption("--sftp-config")]
    public string? SftpConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}