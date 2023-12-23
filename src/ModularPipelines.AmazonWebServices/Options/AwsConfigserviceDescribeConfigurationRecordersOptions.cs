using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "describe-configuration-recorders")]
public record AwsConfigserviceDescribeConfigurationRecordersOptions : AwsOptions
{
    [CommandSwitch("--configuration-recorder-names")]
    public string[]? ConfigurationRecorderNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}