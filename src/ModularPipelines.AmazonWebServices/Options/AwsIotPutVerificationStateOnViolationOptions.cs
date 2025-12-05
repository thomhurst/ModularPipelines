using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "put-verification-state-on-violation")]
public record AwsIotPutVerificationStateOnViolationOptions(
[property: CliOption("--violation-id")] string ViolationId,
[property: CliOption("--verification-state")] string VerificationState
) : AwsOptions
{
    [CliOption("--verification-state-description")]
    public string? VerificationStateDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}