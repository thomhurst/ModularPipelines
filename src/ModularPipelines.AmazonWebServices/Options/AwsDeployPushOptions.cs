using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "push")]
public record AwsDeployPushOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--s3-location")] string S3Location
) : AwsOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}