using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-operations-for-resource")]
public record AwsLightsailGetOperationsForResourceOptions(
[property: CliOption("--resource-name")] string ResourceName
) : AwsOptions
{
    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}