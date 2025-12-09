using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-dataset")]
public record AwsPersonalizeCreateDatasetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--schema-arn")] string SchemaArn,
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn,
[property: CliOption("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}