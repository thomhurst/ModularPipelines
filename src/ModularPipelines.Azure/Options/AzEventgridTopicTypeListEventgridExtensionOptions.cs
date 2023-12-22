using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "topic-type", "list", "(eventgrid", "extension)")]
public record AzEventgridTopicTypeListEventgridExtensionOptions : AzOptions;