using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "content", "update")]
public record GcloudDataplexContentUpdateOptions(
[property: CliArgument] string Content,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--data-text")]
    public string? DataText { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--kernel-type")]
    public string? KernelType { get; set; }

    [CliFlag("PYTHON3")]
    public bool? PYTHON3 { get; set; }

    [CliOption("--query-engine")]
    public string? QueryEngine { get; set; }

    [CliFlag("SPARK")]
    public bool? Spark { get; set; }
}