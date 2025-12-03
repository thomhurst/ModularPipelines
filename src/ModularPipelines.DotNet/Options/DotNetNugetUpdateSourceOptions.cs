using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetUpdateSourceOptions : DotNetOptions
{
    public DotNetNugetUpdateSourceOptions(
        string name
    )
    {
        CommandParts = ["nuget", "update", "source", "<NAME>"];

        Name = name;
    }

    [CliArgument(Name = "<NAME>")]
    public string? Name { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

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
