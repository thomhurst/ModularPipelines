using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wisdom", "update-quick-response")]
public record AwsWisdomUpdateQuickResponseOptions(
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId,
[property: CommandSwitch("--quick-response-id")] string QuickResponseId
) : AwsOptions
{
    [CommandSwitch("--channels")]
    public string[]? Channels { get; set; }

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--grouping-configuration")]
    public string? GroupingConfiguration { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--shortcut-key")]
    public string? ShortcutKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}