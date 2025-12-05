using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "disassociate-health-check")]
public record AwsShieldDisassociateHealthCheckOptions(
[property: CliOption("--protection-id")] string ProtectionId,
[property: CliOption("--health-check-arn")] string HealthCheckArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}