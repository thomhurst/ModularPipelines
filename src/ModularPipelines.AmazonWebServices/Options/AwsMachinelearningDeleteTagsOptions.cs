using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "delete-tags")]
public record AwsMachinelearningDeleteTagsOptions(
[property: CliOption("--tag-keys")] string[] TagKeys,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}