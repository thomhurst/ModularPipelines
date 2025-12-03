using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "update-case")]
public record AwsConnectcasesUpdateCaseOptions(
[property: CliOption("--case-id")] string CaseId,
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--fields")] string[] Fields
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}