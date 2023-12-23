using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-custom-entity-type")]
public record AwsGlueCreateCustomEntityTypeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--regex-string")] string RegexString
) : AwsOptions
{
    [CommandSwitch("--context-words")]
    public string[]? ContextWords { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}