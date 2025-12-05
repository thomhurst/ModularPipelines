using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "get-session-embed-url")]
public record AwsQuicksightGetSessionEmbedUrlOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId
) : AwsOptions
{
    [CliOption("--entry-point")]
    public string? EntryPoint { get; set; }

    [CliOption("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CliOption("--user-arn")]
    public string? UserArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}