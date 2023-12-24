using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "delete-redshift-idc-application")]
public record AwsRedshiftDeleteRedshiftIdcApplicationOptions(
[property: CommandSwitch("--redshift-idc-application-arn")] string RedshiftIdcApplicationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}