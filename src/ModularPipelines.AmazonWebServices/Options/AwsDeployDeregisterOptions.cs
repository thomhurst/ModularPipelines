using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "deregister")]
public record AwsDeployDeregisterOptions(
[property: CliOption("--instance-name")] string InstanceName
) : AwsOptions
{
    [CliFlag("--no-delete-iam-user")]
    public bool? NoDeleteIamUser { get; set; }
}