using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "list-service-versions")]
public record AwsSnowballListServiceVersionsOptions(
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--dependent-services")]
    public string[]? DependentServices { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}