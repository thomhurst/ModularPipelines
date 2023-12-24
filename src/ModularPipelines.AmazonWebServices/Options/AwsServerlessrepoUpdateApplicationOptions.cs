using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("serverlessrepo", "update-application")]
public record AwsServerlessrepoUpdateApplicationOptions(
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--author")]
    public string? Author { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--home-page-url")]
    public string? HomePageUrl { get; set; }

    [CommandSwitch("--labels")]
    public string[]? Labels { get; set; }

    [CommandSwitch("--readme-body")]
    public string? ReadmeBody { get; set; }

    [CommandSwitch("--readme-url")]
    public string? ReadmeUrl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}