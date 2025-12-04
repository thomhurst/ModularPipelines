using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "create")]
public record AzStaticwebappCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--api-location")]
    public string? ApiLocation { get; set; }

    [CliOption("--app-location")]
    public string? AppLocation { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--login-with-ado")]
    public bool? LoginWithAdo { get; set; }

    [CliFlag("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--output-location")]
    public string? OutputLocation { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}