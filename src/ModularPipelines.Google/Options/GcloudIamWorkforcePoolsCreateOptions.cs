using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "create")]
public record GcloudIamWorkforcePoolsCreateOptions(
[property: CliArgument] string WorkforcePool,
[property: CliArgument] string Location,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliOption("--allowed-services")]
    public string[]? AllowedServices { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disable-programmatic-signin")]
    public bool? DisableProgrammaticSignin { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--session-duration")]
    public string? SessionDuration { get; set; }
}