using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "create")]
public record GcloudIamWorkforcePoolsCreateOptions(
[property: PositionalArgument] string WorkforcePool,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [CommandSwitch("--allowed-services")]
    public string[]? AllowedServices { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disable-programmatic-signin")]
    public bool? DisableProgrammaticSignin { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--session-duration")]
    public string? SessionDuration { get; set; }
}