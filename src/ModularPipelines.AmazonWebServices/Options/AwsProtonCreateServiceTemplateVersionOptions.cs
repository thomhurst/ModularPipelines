using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-service-template-version")]
public record AwsProtonCreateServiceTemplateVersionOptions(
[property: CliOption("--compatible-environment-templates")] string[] CompatibleEnvironmentTemplates,
[property: CliOption("--source")] string Source,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--major-version")]
    public string? MajorVersion { get; set; }

    [CliOption("--supported-component-sources")]
    public string[]? SupportedComponentSources { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}