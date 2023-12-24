using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-domain-entry")]
public record AwsLightsailDeleteDomainEntryOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--domain-entry")] string DomainEntry
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}