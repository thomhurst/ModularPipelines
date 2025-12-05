using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-core-definition")]
public record AwsGreengrassGetCoreDefinitionOptions(
[property: CliOption("--core-definition-id")] string CoreDefinitionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}