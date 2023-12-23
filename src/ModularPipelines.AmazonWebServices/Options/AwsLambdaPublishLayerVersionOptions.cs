using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "publish-layer-version")]
public record AwsLambdaPublishLayerVersionOptions(
[property: CommandSwitch("--layer-name")] string LayerName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--compatible-runtimes")]
    public string[]? CompatibleRuntimes { get; set; }

    [CommandSwitch("--license-info")]
    public string? LicenseInfo { get; set; }

    [CommandSwitch("--compatible-architectures")]
    public string[]? CompatibleArchitectures { get; set; }

    [CommandSwitch("--zip-file")]
    public string? ZipFile { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}