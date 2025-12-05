using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "evaluate-session")]
public record AwsVoiceIdEvaluateSessionOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--session-name-or-id")] string SessionNameOrId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}