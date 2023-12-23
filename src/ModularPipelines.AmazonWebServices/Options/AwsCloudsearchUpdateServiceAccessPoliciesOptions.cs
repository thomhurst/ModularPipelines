using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "update-service-access-policies")]
public record AwsCloudsearchUpdateServiceAccessPoliciesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--access-policies")] string AccessPolicies
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}