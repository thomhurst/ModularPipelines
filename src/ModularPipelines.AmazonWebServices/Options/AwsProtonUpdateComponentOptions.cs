using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-component")]
public record AwsProtonUpdateComponentOptions(
[property: CommandSwitch("--deployment-type")] string DeploymentType,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--service-instance-name")]
    public string? ServiceInstanceName { get; set; }

    [CommandSwitch("--service-name")]
    public string? ServiceName { get; set; }

    [CommandSwitch("--service-spec")]
    public string? ServiceSpec { get; set; }

    [CommandSwitch("--template-file")]
    public string? TemplateFile { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}