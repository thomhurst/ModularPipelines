using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qconnect", "create-quick-response")]
public record AwsQconnectCreateQuickResponseOptions(
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--channels")]
    public string[]? Channels { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--grouping-configuration")]
    public string? GroupingConfiguration { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--shortcut-key")]
    public string? ShortcutKey { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}