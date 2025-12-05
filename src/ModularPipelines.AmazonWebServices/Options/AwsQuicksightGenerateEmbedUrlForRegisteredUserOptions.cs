using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "generate-embed-url-for-registered-user")]
public record AwsQuicksightGenerateEmbedUrlForRegisteredUserOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--user-arn")] string UserArn,
[property: CliOption("--experience-configuration")] string ExperienceConfiguration
) : AwsOptions
{
    [CliOption("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CliOption("--allowed-domains")]
    public string[]? AllowedDomains { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}