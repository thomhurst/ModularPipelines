using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dynamodb", "list-imports")]
public record AwsDynamodbListImportsOptions : AwsOptions
{
    [CliOption("--table-arn")]
    public string? TableArn { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}