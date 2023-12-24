using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("serverlessrepo", "create-application-version")]
public record AwsServerlessrepoCreateApplicationVersionOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--semantic-version")] string SemanticVersion
) : AwsOptions
{
    [CommandSwitch("--source-code-archive-url")]
    public string? SourceCodeArchiveUrl { get; set; }

    [CommandSwitch("--source-code-url")]
    public string? SourceCodeUrl { get; set; }

    [CommandSwitch("--template-body")]
    public string? TemplateBody { get; set; }

    [CommandSwitch("--template-url")]
    public string? TemplateUrl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}