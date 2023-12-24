using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "create-service-instance")]
public record AwsProtonCreateServiceInstanceOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--spec")] string Spec
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--template-major-version")]
    public string? TemplateMajorVersion { get; set; }

    [CommandSwitch("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}