using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic", "key", "regenerate", "(eventgrid", "extension)")]
public record AzEventgridTopicKeyRegenerateEventgridExtensionOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;