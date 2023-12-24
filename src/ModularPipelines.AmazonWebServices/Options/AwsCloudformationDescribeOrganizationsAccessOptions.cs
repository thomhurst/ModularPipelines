using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "describe-organizations-access")]
public record AwsCloudformationDescribeOrganizationsAccessOptions : AwsOptions
{
    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}