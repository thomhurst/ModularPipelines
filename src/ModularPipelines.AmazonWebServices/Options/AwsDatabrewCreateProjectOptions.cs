using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "create-project")]
public record AwsDatabrewCreateProjectOptions(
[property: CliOption("--dataset-name")] string DatasetName,
[property: CliOption("--name")] string Name,
[property: CliOption("--recipe-name")] string RecipeName,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--sample")]
    public string? Sample { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}