using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic", "get-organization-api-key")]
public record AzElasticGetOrganizationApiKeyOptions : AzOptions
{
    [CommandSwitch("--email-id")]
    public string? EmailId { get; set; }
}