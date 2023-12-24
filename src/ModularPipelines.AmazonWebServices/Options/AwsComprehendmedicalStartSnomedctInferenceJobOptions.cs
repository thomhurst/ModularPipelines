using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehendmedical", "start-snomedct-inference-job")]
public record AwsComprehendmedicalStartSnomedctInferenceJobOptions(
[property: CommandSwitch("--input-data-config")] string InputDataConfig,
[property: CommandSwitch("--output-data-config")] string OutputDataConfig,
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}