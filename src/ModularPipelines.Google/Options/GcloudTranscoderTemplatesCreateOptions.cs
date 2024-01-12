using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcoder", "templates", "create")]
public record GcloudTranscoderTemplatesCreateOptions(
[property: PositionalArgument] string TemplateId,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--json")] string Json
) : GcloudOptions
{
    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}