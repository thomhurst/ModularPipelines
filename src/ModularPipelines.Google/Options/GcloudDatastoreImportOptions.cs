using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "import")]
public record GcloudDatastoreImportOptions(
[property: PositionalArgument] string InputUrl
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