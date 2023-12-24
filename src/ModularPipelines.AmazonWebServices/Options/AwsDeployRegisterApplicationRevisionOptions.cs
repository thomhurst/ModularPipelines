using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "register-application-revision")]
public record AwsDeployRegisterApplicationRevisionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--s3-location")]
    public string? S3Location { get; set; }

    [CommandSwitch("--github-location")]
    public string? GithubLocation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}