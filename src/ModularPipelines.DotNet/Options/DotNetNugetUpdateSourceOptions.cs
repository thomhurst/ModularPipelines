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
    public string? Source { get; set; }

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
