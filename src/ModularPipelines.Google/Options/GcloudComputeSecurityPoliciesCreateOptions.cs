using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "create")]
public record GcloudComputeSecurityPoliciesCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--file-format")]
    public string? FileFormat { get; set; }

    [CliOption("--file-name")]
    public string? FileName { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}