using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-access-policy")]
public record AwsIotsitewiseUpdateAccessPolicyOptions(
[property: CommandSwitch("--access-policy-id")] string AccessPolicyId,
[property: CommandSwitch("--access-policy-identity")] string AccessPolicyIdentity,
[property: CommandSwitch("--access-policy-resource")] string AccessPolicyResource,
[property: CommandSwitch("--access-policy-permission")] string AccessPolicyPermission
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}