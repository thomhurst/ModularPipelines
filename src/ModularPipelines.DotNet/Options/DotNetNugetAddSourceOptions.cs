using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetAddSourceOptions : DotNetOptions
{
    public DotNetNugetAddSourceOptions(
        string packageSourcePath
    )
    {
        CommandParts = ["nuget", "add", "source", "<PACKAGE_SOURCE_PATH>"];

        PackageSourcePath = packageSourcePath;
    }

    [PositionalArgument(PlaceholderName = "<PACKAGE_SOURCE_PATH>")]
    public string? PackageSourcePath { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--username")]
    public virtual string? Username { get; set; }

    [CommandSwitch("--password")]
    public virtual string? Password { get; set; }

    [BooleanCommandSwitch("--store-password-in-clear-text")]
    public virtual bool? StorePasswordInClearText { get; set; }

    [CommandSwitch("--valid-authentication-types")]
    public virtual string? ValidAuthenticationTypes { get; set; }

    [CommandSwitch("--configfile")]
    public virtual string? Configfile { get; set; }
}
