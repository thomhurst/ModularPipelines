using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "deregister")]
public record AwsDeployDeregisterOptions(
[property: CommandSwitch("--instance-name")] string InstanceName
) : AwsOptions
{
    [BooleanCommandSwitch("--no-delete-iam-user")]
    public bool? NoDeleteIamUser { get; set; }
}