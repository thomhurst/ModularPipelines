using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-app-block")]
public record AwsAppstreamCreateAppBlockOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--source-s3-location")] string SourceS3Location
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--setup-script-details")]
    public string? SetupScriptDetails { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--post-setup-script-details")]
    public string? PostSetupScriptDetails { get; set; }

    [CliOption("--packaging-type")]
    public string? PackagingType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}