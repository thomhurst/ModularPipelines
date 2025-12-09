using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "create-related-item")]
public record AwsConnectcasesCreateRelatedItemOptions(
[property: CliOption("--case-id")] string CaseId,
[property: CliOption("--content")] string Content,
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--performed-by")]
    public string? PerformedBy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}