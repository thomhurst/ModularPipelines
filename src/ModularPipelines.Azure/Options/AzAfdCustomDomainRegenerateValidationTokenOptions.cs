using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "custom-domain", "regenerate-validation-token")]
public record AzAfdCustomDomainRegenerateValidationTokenOptions : AzOptions
{
    [CliOption("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}