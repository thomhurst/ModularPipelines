using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "list-data-lakes")]
public record AwsSecuritylakeListDataLakesOptions : AwsOptions
{
    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}