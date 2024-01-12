using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "export")]
public record GcloudDatastoreExportOptions(
[property: PositionalArgument] string OutputUrlPrefix
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--kinds")]
    public string[]? Kinds { get; set; }

    [CommandSwitch("--namespaces")]
    public string[]? Namespaces { get; set; }

    [CommandSwitch("--operation-labels")]
    public string[]? OperationLabels { get; set; }
}