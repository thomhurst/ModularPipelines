using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehendmedical", "start-phi-detection-job")]
public record AwsComprehendmedicalStartPhiDetectionJobOptions(
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}