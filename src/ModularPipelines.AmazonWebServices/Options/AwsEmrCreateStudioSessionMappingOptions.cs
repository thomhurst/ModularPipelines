using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "create-studio-session-mapping")]
public record AwsEmrCreateStudioSessionMappingOptions(
[property: CommandSwitch("--studio-id")] string StudioId,
[property: CommandSwitch("--identity-type")] string IdentityType,
[property: CommandSwitch("--session-policy-arn")] string SessionPolicyArn
) : AwsOptions
{
    [CommandSwitch("--identity-id")]
    public string? IdentityId { get; set; }

    [CommandSwitch("--identity-name")]
    public string? IdentityName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}