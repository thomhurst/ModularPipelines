using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "push")]
public record AwsDeployPushOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--s3-location")] string S3Location
) : AwsOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }
}