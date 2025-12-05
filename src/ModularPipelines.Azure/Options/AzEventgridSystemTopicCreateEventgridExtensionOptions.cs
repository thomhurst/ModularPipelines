using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "system-topic", "create", "(eventgrid", "extension)")]
public record AzEventgridSystemTopicCreateEventgridExtensionOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source")] string Source,
[property: CliOption("--topic-type")] string TopicType
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}