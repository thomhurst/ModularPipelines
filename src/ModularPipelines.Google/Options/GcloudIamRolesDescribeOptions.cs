using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "roles", "describe")]
public record GcloudIamRolesDescribeOptions(
[property: PositionalArgument] string RoleId
) : GcloudOptions
{
    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}