using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "describe-fraudster")]
public record AwsVoiceIdDescribeFraudsterOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--fraudster-id")] string FraudsterId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}