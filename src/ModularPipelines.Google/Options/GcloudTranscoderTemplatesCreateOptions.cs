using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcoder", "templates", "create")]
public record GcloudTranscoderTemplatesCreateOptions(
[property: CliArgument] string TemplateId,
[property: CliArgument] string Location,
[property: CliOption("--file")] string File,
[property: CliOption("--json")] string Json
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}