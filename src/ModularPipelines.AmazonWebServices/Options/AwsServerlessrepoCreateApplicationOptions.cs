using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("serverlessrepo", "create-application")]
public record AwsServerlessrepoCreateApplicationOptions(
[property: CommandSwitch("--author")] string Author,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--home-page-url")]
    public string? HomePageUrl { get; set; }

    [CommandSwitch("--labels")]
    public string[]? Labels { get; set; }

    [CommandSwitch("--license-body")]
    public string? LicenseBody { get; set; }

    [CommandSwitch("--license-url")]
    public string? LicenseUrl { get; set; }

    [CommandSwitch("--readme-body")]
    public string? ReadmeBody { get; set; }

    [CommandSwitch("--readme-url")]
    public string? ReadmeUrl { get; set; }

    [CommandSwitch("--semantic-version")]
    public string? SemanticVersion { get; set; }

    [CommandSwitch("--source-code-archive-url")]
    public string? SourceCodeArchiveUrl { get; set; }

    [CommandSwitch("--source-code-url")]
    public string? SourceCodeUrl { get; set; }

    [CommandSwitch("--spdx-license-id")]
    public string? SpdxLicenseId { get; set; }

    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--template-url")]
    public string? TemplateUrl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}