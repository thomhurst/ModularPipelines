using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic", "create")]
public record AzEventgridSystemTopicCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--topic-type")] string TopicType
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}