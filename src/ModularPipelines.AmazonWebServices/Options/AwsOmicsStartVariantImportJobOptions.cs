using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "start-variant-import-job")]
public record AwsOmicsStartVariantImportJobOptions(
[property: CliOption("--destination-name")] string DestinationName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--annotation-fields")]
    public IEnumerable<KeyValue>? AnnotationFields { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}