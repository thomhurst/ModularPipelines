using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic", "get-organization-api-key")]
public record AzElasticGetOrganizationApiKeyOptions : AzOptions
{
    [CommandSwitch("--email-id")]
    public string? EmailId { get; set; }
}