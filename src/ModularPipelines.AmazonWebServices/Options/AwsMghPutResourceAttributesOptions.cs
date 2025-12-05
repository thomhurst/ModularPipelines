using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgh", "put-resource-attributes")]
public record AwsMghPutResourceAttributesOptions(
[property: CliOption("--progress-update-stream")] string ProgressUpdateStream,
[property: CliOption("--migration-task-name")] string MigrationTaskName,
[property: CliOption("--resource-attribute-list")] string[] ResourceAttributeList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}