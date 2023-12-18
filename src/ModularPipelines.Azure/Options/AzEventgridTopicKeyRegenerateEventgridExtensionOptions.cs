using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic", "key", "regenerate", "(eventgrid", "extension)")]
public record AzEventgridTopicKeyRegenerateEventgridExtensionOptions(
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;