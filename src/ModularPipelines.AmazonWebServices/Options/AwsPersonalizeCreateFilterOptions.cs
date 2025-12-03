using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-filter")]
public record AwsPersonalizeCreateFilterOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--dataset-group-arn")] string DatasetGroupArn,
[property: CliOption("--filter-expression")] string FilterExpression
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}