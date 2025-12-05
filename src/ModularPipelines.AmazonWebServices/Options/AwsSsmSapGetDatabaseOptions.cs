using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-sap", "get-database")]
public record AwsSsmSapGetDatabaseOptions : AwsOptions
{
    [CliOption("--application-id")]
    public string? ApplicationId { get; set; }

    [CliOption("--component-id")]
    public string? ComponentId { get; set; }

    [CliOption("--database-id")]
    public string? DatabaseId { get; set; }

    [CliOption("--database-arn")]
    public string? DatabaseArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}