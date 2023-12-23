using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "is-authorized-with-token")]
public record AwsVerifiedpermissionsIsAuthorizedWithTokenOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId
) : AwsOptions
{
    [CommandSwitch("--identity-token")]
    public string? IdentityToken { get; set; }

    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--entities")]
    public string? Entities { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}