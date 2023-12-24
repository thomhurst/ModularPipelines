using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "describe-account-attributes")]
public record AwsRedshiftDescribeAccountAttributesOptions : AwsOptions
{
    [CommandSwitch("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}