using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "set-identity-mail-from-domain")]
public record AwsSesSetIdentityMailFromDomainOptions(
[property: CliOption("--identity")] string Identity
) : AwsOptions
{
    [CliOption("--mail-from-domain")]
    public string? MailFromDomain { get; set; }

    [CliOption("--behavior-on-mx-failure")]
    public string? BehaviorOnMxFailure { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}