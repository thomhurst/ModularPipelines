using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "describe-lifecycle-configuration")]
public record AwsEfsDescribeLifecycleConfigurationOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}