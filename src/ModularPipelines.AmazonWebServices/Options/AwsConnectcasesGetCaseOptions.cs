using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "get-case")]
public record AwsConnectcasesGetCaseOptions(
[property: CliOption("--case-id")] string CaseId,
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--fields")] string[] Fields
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}