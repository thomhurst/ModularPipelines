using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "create-connector")]
public record AwsTransferCreateConnectorOptions(
[property: CliOption("--url")] string Url,
[property: CliOption("--access-role")] string AccessRole
) : AwsOptions
{
    [CliOption("--as2-config")]
    public string? As2Config { get; set; }

    [CliOption("--logging-role")]
    public string? LoggingRole { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--sftp-config")]
    public string? SftpConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}