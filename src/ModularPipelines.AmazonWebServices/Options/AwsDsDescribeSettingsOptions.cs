using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "describe-settings")]
public record AwsDsDescribeSettingsOptions(
[property: CliOption("--directory-id")] string DirectoryId
) : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}