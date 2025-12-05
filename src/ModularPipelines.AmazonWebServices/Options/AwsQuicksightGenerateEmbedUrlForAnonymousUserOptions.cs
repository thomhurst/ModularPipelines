using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "generate-embed-url-for-anonymous-user")]
public record AwsQuicksightGenerateEmbedUrlForAnonymousUserOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--authorized-resource-arns")] string[] AuthorizedResourceArns,
[property: CliOption("--experience-configuration")] string ExperienceConfiguration
) : AwsOptions
{
    [CliOption("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CliOption("--session-tags")]
    public string[]? SessionTags { get; set; }

    [CliOption("--allowed-domains")]
    public string[]? AllowedDomains { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}