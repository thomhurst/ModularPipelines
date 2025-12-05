using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "create-cross-account-authorization")]
public record AwsRoute53RecoveryReadinessCreateCrossAccountAuthorizationOptions(
[property: CliOption("--cross-account-authorization")] string CrossAccountAuthorization
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}