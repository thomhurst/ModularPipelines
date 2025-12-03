using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "create-case")]
public record AwsConnectcasesCreateCaseOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--fields")] string[] Fields,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}