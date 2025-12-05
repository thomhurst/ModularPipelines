using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-ops-metadata")]
public record AwsSsmUpdateOpsMetadataOptions(
[property: CliOption("--ops-metadata-arn")] string OpsMetadataArn
) : AwsOptions
{
    [CliOption("--metadata-to-update")]
    public IEnumerable<KeyValue>? MetadataToUpdate { get; set; }

    [CliOption("--keys-to-delete")]
    public string[]? KeysToDelete { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}