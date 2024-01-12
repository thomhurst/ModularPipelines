using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "roles", "copy")]
public record GcloudIamRolesCopyOptions : GcloudOptions
{
    [CommandSwitch("--dest-organization")]
    public string? DestOrganization { get; set; }

    [CommandSwitch("--dest-project")]
    public string? DestProject { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--source-organization")]
    public string? SourceOrganization { get; set; }

    [CommandSwitch("--source-project")]
    public string? SourceProject { get; set; }
}