using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "get-blacklist-reports")]
public record AwsPinpointEmailGetBlacklistReportsOptions(
[property: CliOption("--blacklist-item-names")] string[] BlacklistItemNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}