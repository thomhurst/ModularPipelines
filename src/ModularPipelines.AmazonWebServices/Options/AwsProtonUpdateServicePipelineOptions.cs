using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-service-pipeline")]
public record AwsProtonUpdateServicePipelineOptions(
[property: CliOption("--deployment-type")] string DeploymentType,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--spec")] string Spec
) : AwsOptions
{
    [CliOption("--template-major-version")]
    public string? TemplateMajorVersion { get; set; }

    [CliOption("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}