using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "update-service-template-version")]
public record AwsProtonUpdateServiceTemplateVersionOptions(
[property: CliOption("--major-version")] string MajorVersion,
[property: CliOption("--minor-version")] string MinorVersion,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--compatible-environment-templates")]
    public string[]? CompatibleEnvironmentTemplates { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--supported-component-sources")]
    public string[]? SupportedComponentSources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}