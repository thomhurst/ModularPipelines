using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "create-service-template-version")]
public record AwsProtonCreateServiceTemplateVersionOptions(
[property: CommandSwitch("--compatible-environment-templates")] string[] CompatibleEnvironmentTemplates,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--major-version")]
    public string? MajorVersion { get; set; }

    [CommandSwitch("--supported-component-sources")]
    public string[]? SupportedComponentSources { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}