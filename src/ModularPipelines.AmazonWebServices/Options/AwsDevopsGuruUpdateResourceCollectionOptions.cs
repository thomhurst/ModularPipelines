using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops-guru", "update-resource-collection")]
public record AwsDevopsGuruUpdateResourceCollectionOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--resource-collection")] string ResourceCollection
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}