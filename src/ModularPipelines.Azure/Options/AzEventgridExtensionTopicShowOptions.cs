using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "extension-topic", "show")]
public record AzEventgridExtensionTopicShowOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions;