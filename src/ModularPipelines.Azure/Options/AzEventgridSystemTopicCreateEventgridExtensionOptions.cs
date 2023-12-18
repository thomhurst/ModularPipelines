using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic", "create", "(eventgrid", "extension)")]
public record AzEventgridSystemTopicCreateEventgridExtensionOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--topic-type")] string TopicType
) : AzOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}