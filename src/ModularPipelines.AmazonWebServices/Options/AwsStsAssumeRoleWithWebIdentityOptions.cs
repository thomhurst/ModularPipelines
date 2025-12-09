using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sts", "assume-role-with-web-identity")]
public record AwsStsAssumeRoleWithWebIdentityOptions(
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--role-session-name")] string RoleSessionName,
[property: CliOption("--web-identity-token")] string WebIdentityToken
) : AwsOptions
{
    [CliOption("--provider-id")]
    public string? ProviderId { get; set; }

    [CliOption("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}