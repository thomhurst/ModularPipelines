using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "replicate-key")]
public record AwsKmsReplicateKeyOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--replica-region")] string ReplicaRegion
) : AwsOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}