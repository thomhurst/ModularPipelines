using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("robomaker", "create-world-generation-job")]
public record AwsRobomakerCreateWorldGenerationJobOptions(
[property: CommandSwitch("--template")] string Template,
[property: CommandSwitch("--world-count")] string WorldCount
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--world-tags")]
    public IEnumerable<KeyValue>? WorldTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}