using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-service-pipeline")]
public record AwsProtonUpdateServicePipelineOptions(
[property: CommandSwitch("--deployment-type")] string DeploymentType,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--spec")] string Spec
) : AwsOptions
{
    [CommandSwitch("--template-major-version")]
    public string? TemplateMajorVersion { get; set; }

    [CommandSwitch("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}