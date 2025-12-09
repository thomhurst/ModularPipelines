using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "search-channels")]
public record AwsChimeSdkMessagingSearchChannelsOptions(
[property: CliOption("--fields")] string[] Fields
) : AwsOptions
{
    [CliOption("--chime-bearer")]
    public string? ChimeBearer { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}