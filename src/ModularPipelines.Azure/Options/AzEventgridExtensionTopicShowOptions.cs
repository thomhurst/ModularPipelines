using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "extension-topic", "show")]
public record AzEventgridExtensionTopicShowOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions;