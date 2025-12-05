using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "update-quick-response")]
public record AwsQconnectUpdateQuickResponseOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--quick-response-id")] string QuickResponseId
) : AwsOptions
{
    [CliOption("--channels")]
    public string[]? Channels { get; set; }

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--grouping-configuration")]
    public string? GroupingConfiguration { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--shortcut-key")]
    public string? ShortcutKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}