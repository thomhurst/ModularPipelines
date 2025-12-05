using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "enable-region")]
public record AwsAccountEnableRegionOptions(
[property: CliOption("--region-name")] string RegionName
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}