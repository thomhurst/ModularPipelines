using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "start-discovery-job")]
public record AwsDatasyncStartDiscoveryJobOptions(
[property: CliOption("--storage-system-arn")] string StorageSystemArn,
[property: CliOption("--collection-duration-minutes")] int CollectionDurationMinutes
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}