using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "delete-studio-session-mapping")]
public record AwsEmrDeleteStudioSessionMappingOptions(
[property: CommandSwitch("--studio-id")] string StudioId,
[property: CommandSwitch("--identity-type")] string IdentityType
) : AwsOptions
{
    [CommandSwitch("--identity-id")]
    public string? IdentityId { get; set; }

    [CommandSwitch("--identity-name")]
    public string? IdentityName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}