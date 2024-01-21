using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetAddSourceOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "<PACKAGE_SOURCE_PATH>")]
    public string? PackageSourcePath { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--store-password-in-clear-text")]
    public bool? StorePasswordInClearText { get; set; }

    [CommandSwitch("--valid-authentication-types")]
    public string? ValidAuthenticationTypes { get; set; }

    [CommandSwitch("--configfile")]
    public string? Configfile { get; set; }
}
