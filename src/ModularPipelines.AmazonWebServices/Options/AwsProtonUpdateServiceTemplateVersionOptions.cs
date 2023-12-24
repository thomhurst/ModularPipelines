using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "update-service-template-version")]
public record AwsProtonUpdateServiceTemplateVersionOptions(
[property: CommandSwitch("--major-version")] string MajorVersion,
[property: CommandSwitch("--minor-version")] string MinorVersion,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--compatible-environment-templates")]
    public string[]? CompatibleEnvironmentTemplates { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--supported-component-sources")]
    public string[]? SupportedComponentSources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}