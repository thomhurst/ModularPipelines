using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "release", "create")]
public record AzApimApiReleaseCreateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--api-revision")] string ApiRevision,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--release-id")]
    public string? ReleaseId { get; set; }
}