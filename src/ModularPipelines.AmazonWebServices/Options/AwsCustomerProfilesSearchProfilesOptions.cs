using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "search-profiles")]
public record AwsCustomerProfilesSearchProfilesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--values")] string[] Values
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--additional-search-keys")]
    public string[]? AdditionalSearchKeys { get; set; }

    [CommandSwitch("--logical-operator")]
    public string? LogicalOperator { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}