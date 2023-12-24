using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-app-block")]
public record AwsAppstreamCreateAppBlockOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source-s3-location")] string SourceS3Location
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--setup-script-details")]
    public string? SetupScriptDetails { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--post-setup-script-details")]
    public string? PostSetupScriptDetails { get; set; }

    [CommandSwitch("--packaging-type")]
    public string? PackagingType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}