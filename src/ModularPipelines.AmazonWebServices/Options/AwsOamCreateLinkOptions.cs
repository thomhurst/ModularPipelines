using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("oam", "create-link")]
public record AwsOamCreateLinkOptions(
[property: CommandSwitch("--label-template")] string LabelTemplate,
[property: CommandSwitch("--resource-types")] string[] ResourceTypes,
[property: CommandSwitch("--sink-identifier")] string SinkIdentifier
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}