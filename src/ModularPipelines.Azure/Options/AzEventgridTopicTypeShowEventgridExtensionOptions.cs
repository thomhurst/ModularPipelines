using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic-type", "show", "(eventgrid", "extension)")]
public record AzEventgridTopicTypeShowEventgridExtensionOptions(
[property: CliOption("--name")] string Name
) : AzOptions;