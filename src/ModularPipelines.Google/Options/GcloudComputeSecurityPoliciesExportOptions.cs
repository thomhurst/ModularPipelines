using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "export")]
public record GcloudComputeSecurityPoliciesExportOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--file-name")] string FileName
) : GcloudOptions
{
    [CommandSwitch("--file-format")]
    public string? FileFormat { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}