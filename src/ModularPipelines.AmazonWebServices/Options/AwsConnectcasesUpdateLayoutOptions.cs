using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "update-layout")]
public record AwsConnectcasesUpdateLayoutOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--layout-id")] string LayoutId
) : AwsOptions
{
    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}