using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "generate-embed-url-for-anonymous-user")]
public record AwsQuicksightGenerateEmbedUrlForAnonymousUserOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--authorized-resource-arns")] string[] AuthorizedResourceArns,
[property: CommandSwitch("--experience-configuration")] string ExperienceConfiguration
) : AwsOptions
{
    [CommandSwitch("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CommandSwitch("--session-tags")]
    public string[]? SessionTags { get; set; }

    [CommandSwitch("--allowed-domains")]
    public string[]? AllowedDomains { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}