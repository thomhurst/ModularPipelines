using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "extension-topic", "show", "(eventgrid", "extension)")]
public record AzEventgridExtensionTopicShowEventgridExtensionOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions;