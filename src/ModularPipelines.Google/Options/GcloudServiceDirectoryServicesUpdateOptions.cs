using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-directory", "services", "update")]
public record GcloudServiceDirectoryServicesUpdateOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Namespace
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }
}