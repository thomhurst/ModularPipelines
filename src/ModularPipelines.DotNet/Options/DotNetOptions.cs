using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetOptions() : CommandLineToolOptions("dotnet")
{
    [CommandSwitch("--property:", SwitchValueSeparator = "")]
    public virtual IEnumerable<KeyValue>? Properties { get; set; }
}
