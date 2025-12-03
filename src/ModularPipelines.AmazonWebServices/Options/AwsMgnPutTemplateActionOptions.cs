using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "put-template-action")]
public record AwsMgnPutTemplateActionOptions(
[property: CliOption("--action-id")] string ActionId,
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--document-identifier")] string DocumentIdentifier,
[property: CliOption("--launch-configuration-template-id")] string LaunchConfigurationTemplateId,
[property: CliOption("--order")] int Order
) : AwsOptions
{
    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--document-version")]
    public string? DocumentVersion { get; set; }

    [CliOption("--external-parameters")]
    public IEnumerable<KeyValue>? ExternalParameters { get; set; }

    [CliOption("--operating-system")]
    public string? OperatingSystem { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--timeout-seconds")]
    public int? TimeoutSeconds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}