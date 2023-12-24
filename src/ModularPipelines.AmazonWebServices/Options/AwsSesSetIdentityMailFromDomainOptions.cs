using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "set-identity-mail-from-domain")]
public record AwsSesSetIdentityMailFromDomainOptions(
[property: CommandSwitch("--identity")] string Identity
) : AwsOptions
{
    [CommandSwitch("--mail-from-domain")]
    public string? MailFromDomain { get; set; }

    [CommandSwitch("--behavior-on-mx-failure")]
    public string? BehaviorOnMxFailure { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}