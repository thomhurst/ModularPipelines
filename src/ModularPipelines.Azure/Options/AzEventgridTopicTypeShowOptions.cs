using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic-type", "show")]
public record AzEventgridTopicTypeShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;