using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "extension-topic", "show", "(eventgrid", "extension)")]
public record AzEventgridExtensionTopicShowEventgridExtensionOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions;