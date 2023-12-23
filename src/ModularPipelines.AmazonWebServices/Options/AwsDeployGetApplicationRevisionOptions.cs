using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "get-application-revision")]
public record AwsDeployGetApplicationRevisionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--s3-location")]
    public string? S3Location { get; set; }

    [CommandSwitch("--github-location")]
    public string? GithubLocation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}