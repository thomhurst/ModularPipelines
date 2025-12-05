using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "search-profiles")]
public record AwsCustomerProfilesSearchProfilesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--values")] string[] Values
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--additional-search-keys")]
    public string[]? AdditionalSearchKeys { get; set; }

    [CliOption("--logical-operator")]
    public string? LogicalOperator { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}