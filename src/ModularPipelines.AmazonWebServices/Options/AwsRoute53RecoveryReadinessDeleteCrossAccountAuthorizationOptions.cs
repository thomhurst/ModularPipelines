using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53-recovery-readiness", "delete-cross-account-authorization")]
public record AwsRoute53RecoveryReadinessDeleteCrossAccountAuthorizationOptions(
[property: CliOption("--cross-account-authorization")] string CrossAccountAuthorization
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}