using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "get-template")]
public record AwsConnectcasesGetTemplateOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}