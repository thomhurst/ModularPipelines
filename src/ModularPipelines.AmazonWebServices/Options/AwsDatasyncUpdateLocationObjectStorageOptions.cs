using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "update-location-object-storage")]
public record AwsDatasyncUpdateLocationObjectStorageOptions(
[property: CliOption("--location-arn")] string LocationArn
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

    [CliOption("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CliOption("--server-certificate")]
    public string? ServerCertificate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}