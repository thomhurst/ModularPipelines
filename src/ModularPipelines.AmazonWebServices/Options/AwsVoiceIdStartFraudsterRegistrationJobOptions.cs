using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "start-fraudster-registration-job")]
public record AwsVoiceIdStartFraudsterRegistrationJobOptions(
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--registration-config")]
    public string? RegistrationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}