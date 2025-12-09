using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "devbox-definition", "show")]
public record AzDevcenterAdminDevboxDefinitionShowOptions : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--devbox-definition-name")]
    public string? DevboxDefinitionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}