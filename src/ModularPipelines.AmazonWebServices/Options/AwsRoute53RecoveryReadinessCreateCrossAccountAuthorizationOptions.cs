using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53-recovery-readiness", "create-cross-account-authorization")]
public record AwsRoute53RecoveryReadinessCreateCrossAccountAuthorizationOptions(
[property: CommandSwitch("--cross-account-authorization")] string CrossAccountAuthorization
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}