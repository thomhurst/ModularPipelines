using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "create-channel")]
public record AwsCloudtrailCreateChannelOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--destinations")] string[] Destinations
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}