using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic-type", "list", "(eventgrid", "extension)")]
public record AzEventgridTopicTypeListEventgridExtensionOptions : AzOptions;