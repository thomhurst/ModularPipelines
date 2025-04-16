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

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? Name { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

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
