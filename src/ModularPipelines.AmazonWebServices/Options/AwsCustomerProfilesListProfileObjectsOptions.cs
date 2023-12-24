using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "list-profile-objects")]
public record AwsCustomerProfilesListProfileObjectsOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--object-type-name")] string ObjectTypeName,
[property: CommandSwitch("--profile-id")] string ProfileId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--object-filter")]
    public string? ObjectFilter { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}