using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "list-lifecycle-execution-resources")]
public record AwsImagebuilderListLifecycleExecutionResourcesOptions(
[property: CliOption("--lifecycle-execution-id")] string LifecycleExecutionId
) : AwsOptions
{
    [CliOption("--parent-resource-id")]
    public string? ParentResourceId { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}