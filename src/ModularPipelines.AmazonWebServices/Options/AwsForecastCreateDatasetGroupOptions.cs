using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-dataset-group")]
public record AwsForecastCreateDatasetGroupOptions(
[property: CliOption("--dataset-group-name")] string DatasetGroupName,
[property: CliOption("--domain")] string Domain
) : AwsOptions
{
    [CliOption("--dataset-arns")]
    public string[]? DatasetArns { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}