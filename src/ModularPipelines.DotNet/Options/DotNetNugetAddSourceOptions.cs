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

    [CliArgument(Name = "<PACKAGE_SOURCE_PATH>")]
    public string? PackageSourcePath { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--username")]
    public virtual string? Username { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliFlag("--store-password-in-clear-text")]
    public virtual bool? StorePasswordInClearText { get; set; }

    [CliOption("--valid-authentication-types")]
    public virtual string? ValidAuthenticationTypes { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }
}
