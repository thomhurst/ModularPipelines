using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-data-source")]
public record AwsDatazoneCreateDataSourceOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-identifier")] string ProjectIdentifier,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--asset-forms-input")]
    public string[]? AssetFormsInput { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--enable-setting")]
    public string? EnableSetting { get; set; }

    [CommandSwitch("--recommendation")]
    public string? Recommendation { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}