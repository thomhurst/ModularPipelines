using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "get-integration")]
public record AwsCustomerProfilesGetIntegrationOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--uri")] string Uri
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}