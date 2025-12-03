using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "export")]
public record GcloudComputeSecurityPoliciesExportOptions(
[property: CliArgument] string Name,
[property: CliOption("--file-name")] string FileName
) : GcloudOptions
{
    [CliOption("--file-format")]
    public string? FileFormat { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}