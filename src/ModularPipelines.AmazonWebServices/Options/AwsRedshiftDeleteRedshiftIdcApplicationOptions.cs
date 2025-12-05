using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-redshift-idc-application")]
public record AwsRedshiftDeleteRedshiftIdcApplicationOptions(
[property: CliOption("--redshift-idc-application-arn")] string RedshiftIdcApplicationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}