using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "get-session-embed-url")]
public record AwsQuicksightGetSessionEmbedUrlOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId
) : AwsOptions
{
    [CommandSwitch("--entry-point")]
    public string? EntryPoint { get; set; }

    [CommandSwitch("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CommandSwitch("--user-arn")]
    public string? UserArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}