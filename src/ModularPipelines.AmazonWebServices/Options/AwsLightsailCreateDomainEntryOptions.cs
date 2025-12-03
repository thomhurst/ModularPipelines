using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-domain-entry")]
public record AwsLightsailCreateDomainEntryOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--domain-entry")] string DomainEntry
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}