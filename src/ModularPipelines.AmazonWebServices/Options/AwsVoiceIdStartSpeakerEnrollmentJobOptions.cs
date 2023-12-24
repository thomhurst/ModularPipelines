using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("voice-id", "start-speaker-enrollment-job")]
public record AwsVoiceIdStartSpeakerEnrollmentJobOptions(
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn,
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--input-data-config")] string InputDataConfig,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--enrollment-config")]
    public string? EnrollmentConfig { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}