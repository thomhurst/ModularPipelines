using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "remove-storage-system")]
public record AwsDatasyncRemoveStorageSystemOptions(
[property: CliOption("--storage-system-arn")] string StorageSystemArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}