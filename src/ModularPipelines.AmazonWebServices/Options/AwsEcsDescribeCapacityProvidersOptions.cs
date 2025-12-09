using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "describe-capacity-providers")]
public record AwsEcsDescribeCapacityProvidersOptions : AwsOptions
{
    [CliOption("--capacity-providers")]
    public string[]? CapacityProviders { get; set; }

    [CliOption("--include")]
    public string[]? Include { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}