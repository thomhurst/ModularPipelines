using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-directory", "services", "create")]
public record GcloudServiceDirectoryServicesCreateOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliArgument] string Namespace
) : GcloudOptions
{
    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }
}