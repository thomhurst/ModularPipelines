using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "create-application")]
public record AwsMgnCreateApplicationOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}