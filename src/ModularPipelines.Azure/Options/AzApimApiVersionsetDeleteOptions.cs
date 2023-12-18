using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "versionset", "delete")]
public record AzApimApiVersionsetDeleteOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--version-set-id")] string VersionSetId
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }
}