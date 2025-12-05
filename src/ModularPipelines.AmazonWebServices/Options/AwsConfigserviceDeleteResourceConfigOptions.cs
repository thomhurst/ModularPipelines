using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-resource-config")]
public record AwsConfigserviceDeleteResourceConfigOptions(
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}