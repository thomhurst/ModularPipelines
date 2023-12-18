using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "create")]
public record AzStaticwebappCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--api-location")]
    public string? ApiLocation { get; set; }

    [CommandSwitch("--app-location")]
    public string? AppLocation { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--login-with-ado")]
    public bool? LoginWithAdo { get; set; }

    [BooleanCommandSwitch("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--output-location")]
    public string? OutputLocation { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}

