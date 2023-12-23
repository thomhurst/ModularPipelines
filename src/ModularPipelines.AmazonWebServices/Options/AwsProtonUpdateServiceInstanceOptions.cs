using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-service-instance")]
public record AwsProtonUpdateServiceInstanceOptions(
[property: CommandSwitch("--deployment-type")] string DeploymentType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--service-name")] string ServiceName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--spec")]
    public string? Spec { get; set; }

    [CommandSwitch("--template-major-version")]
    public string? TemplateMajorVersion { get; set; }

    [CommandSwitch("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}