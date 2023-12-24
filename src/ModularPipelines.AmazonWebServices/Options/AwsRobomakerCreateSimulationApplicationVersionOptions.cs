using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "create-simulation-application-version")]
public record AwsRobomakerCreateSimulationApplicationVersionOptions(
[property: CommandSwitch("--application")] string Application
) : AwsOptions
{
    [CommandSwitch("--current-revision-id")]
    public string? CurrentRevisionId { get; set; }

    [CommandSwitch("--s3-etags")]
    public string[]? S3Etags { get; set; }

    [CommandSwitch("--image-digest")]
    public string? ImageDigest { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}