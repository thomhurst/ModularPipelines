using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "roles", "copy")]
public record GcloudIamRolesCopyOptions : GcloudOptions
{
    [CliOption("--dest-organization")]
    public string? DestOrganization { get; set; }

    [CliOption("--dest-project")]
    public string? DestProject { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--source-organization")]
    public string? SourceOrganization { get; set; }

    [CliOption("--source-project")]
    public string? SourceProject { get; set; }
}