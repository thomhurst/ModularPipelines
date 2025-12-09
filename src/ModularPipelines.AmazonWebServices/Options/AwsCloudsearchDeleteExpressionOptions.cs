using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "delete-expression")]
public record AwsCloudsearchDeleteExpressionOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--expression-name")] string ExpressionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}