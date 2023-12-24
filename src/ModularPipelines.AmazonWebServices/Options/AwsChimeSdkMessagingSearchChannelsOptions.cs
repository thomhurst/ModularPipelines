using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "search-channels")]
public record AwsChimeSdkMessagingSearchChannelsOptions(
[property: CommandSwitch("--fields")] string[] Fields
) : AwsOptions
{
    [CommandSwitch("--chime-bearer")]
    public string? ChimeBearer { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}