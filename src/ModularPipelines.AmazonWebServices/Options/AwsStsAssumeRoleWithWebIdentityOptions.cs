using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sts", "assume-role-with-web-identity")]
public record AwsStsAssumeRoleWithWebIdentityOptions(
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--role-session-name")] string RoleSessionName,
[property: CommandSwitch("--web-identity-token")] string WebIdentityToken
) : AwsOptions
{
    [CommandSwitch("--provider-id")]
    public string? ProviderId { get; set; }

    [CommandSwitch("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}