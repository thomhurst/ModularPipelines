using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migration-hub-refactor-spaces", "tag-resource")]
public record AwsMigrationHubRefactorSpacesTagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tags")] IEnumerable<KeyValue> Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}