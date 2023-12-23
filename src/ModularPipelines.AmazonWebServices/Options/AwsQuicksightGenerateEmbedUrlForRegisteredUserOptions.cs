using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "generate-embed-url-for-registered-user")]
public record AwsQuicksightGenerateEmbedUrlForRegisteredUserOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--user-arn")] string UserArn,
[property: CommandSwitch("--experience-configuration")] string ExperienceConfiguration
) : AwsOptions
{
    [CommandSwitch("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CommandSwitch("--allowed-domains")]
    public string[]? AllowedDomains { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}