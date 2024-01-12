using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "namespaces", "create")]
public record GcloudServiceDirectoryNamespacesCreateOptions(
[property: PositionalArgument] string Namespace,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}