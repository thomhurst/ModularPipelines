using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "import")]
public record GcloudDatastoreImportOptions(
[property: CliArgument] string InputUrl
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--kinds")]
    public string[]? Kinds { get; set; }

    [CliOption("--namespaces")]
    public string[]? Namespaces { get; set; }

    [CliOption("--operation-labels")]
    public string[]? OperationLabels { get; set; }
}