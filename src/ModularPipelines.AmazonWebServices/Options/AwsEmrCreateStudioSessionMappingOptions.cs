using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "create-studio-session-mapping")]
public record AwsEmrCreateStudioSessionMappingOptions(
[property: CliOption("--studio-id")] string StudioId,
[property: CliOption("--identity-type")] string IdentityType,
[property: CliOption("--session-policy-arn")] string SessionPolicyArn
) : AwsOptions
{
    [CliOption("--identity-id")]
    public string? IdentityId { get; set; }

    [CliOption("--identity-name")]
    public string? IdentityName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}