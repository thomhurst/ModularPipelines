using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "delete-resource-definition")]
public record AwsGreengrassDeleteResourceDefinitionOptions(
[property: CliOption("--resource-definition-id")] string ResourceDefinitionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}