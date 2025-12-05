using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "publish-layer-version")]
public record AwsLambdaPublishLayerVersionOptions(
[property: CliOption("--layer-name")] string LayerName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--compatible-runtimes")]
    public string[]? CompatibleRuntimes { get; set; }

    [CliOption("--license-info")]
    public string? LicenseInfo { get; set; }

    [CliOption("--compatible-architectures")]
    public string[]? CompatibleArchitectures { get; set; }

    [CliOption("--zip-file")]
    public string? ZipFile { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}