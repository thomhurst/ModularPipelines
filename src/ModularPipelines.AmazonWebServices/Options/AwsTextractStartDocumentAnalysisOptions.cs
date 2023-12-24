using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("textract", "start-document-analysis")]
public record AwsTextractStartDocumentAnalysisOptions(
[property: CommandSwitch("--document-location")] string DocumentLocation,
[property: CommandSwitch("--feature-types")] string[] FeatureTypes
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--job-tag")]
    public string? JobTag { get; set; }

    [CommandSwitch("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CommandSwitch("--output-config")]
    public string? OutputConfig { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--queries-config")]
    public string? QueriesConfig { get; set; }

    [CommandSwitch("--adapters-config")]
    public string? AdaptersConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}