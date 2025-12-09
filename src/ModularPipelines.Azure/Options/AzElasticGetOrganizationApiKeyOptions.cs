using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic", "get-organization-api-key")]
public record AzElasticGetOrganizationApiKeyOptions : AzOptions
{
    [CliOption("--email-id")]
    public string? EmailId { get; set; }
}