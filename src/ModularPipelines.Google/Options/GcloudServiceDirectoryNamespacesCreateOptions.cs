using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "namespaces", "create")]
public record GcloudServiceDirectoryNamespacesCreateOptions(
[property: CliArgument] string Namespace,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}