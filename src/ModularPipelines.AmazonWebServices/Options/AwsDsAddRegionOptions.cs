using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "add-region")]
public record AwsDsAddRegionOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--region-name")] string RegionName,
[property: CliOption("--vpc-settings")] string VpcSettings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}