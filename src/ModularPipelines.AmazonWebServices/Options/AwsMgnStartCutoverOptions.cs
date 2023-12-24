using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "start-cutover")]
public record AwsMgnStartCutoverOptions(
[property: CommandSwitch("--source-server-ids")] string[] SourceServerIds
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}