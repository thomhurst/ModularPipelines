using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "create-quick-response")]
public record AwsWisdomCreateQuickResponseOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--channels")]
    public string[]? Channels { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--grouping-configuration")]
    public string? GroupingConfiguration { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--shortcut-key")]
    public string? ShortcutKey { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}