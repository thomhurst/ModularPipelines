using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "get-layout")]
public record AwsConnectcasesGetLayoutOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--layout-id")] string LayoutId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}