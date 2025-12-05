using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "get-provider-service")]
public record AwsEntityresolutionGetProviderServiceOptions(
[property: CliOption("--provider-name")] string ProviderName,
[property: CliOption("--provider-service-name")] string ProviderServiceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}