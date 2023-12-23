using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "put-verification-state-on-violation")]
public record AwsIotPutVerificationStateOnViolationOptions(
[property: CommandSwitch("--violation-id")] string ViolationId,
[property: CommandSwitch("--verification-state")] string VerificationState
) : AwsOptions
{
    [CommandSwitch("--verification-state-description")]
    public string? VerificationStateDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}