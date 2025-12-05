using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-object-storage")]
public record AwsDatasyncCreateLocationObjectStorageOptions(
[property: CliOption("--server-hostname")] string ServerHostname,
[property: CliOption("--bucket-name")] string BucketName,
[property: CliOption("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CliOption("--server-port")]
    public int? ServerPort { get; set; }

    [CliOption("--server-protocol")]
    public string? ServerProtocol { get; set; }

    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--access-key")]
    public string? AccessKey { get; set; }

    [CliOption("--secret-key")]
    public string? SecretKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--server-certificate")]
    public string? ServerCertificate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}