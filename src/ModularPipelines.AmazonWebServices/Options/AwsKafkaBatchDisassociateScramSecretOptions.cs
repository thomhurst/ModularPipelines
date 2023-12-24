using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "batch-disassociate-scram-secret")]
public record AwsKafkaBatchDisassociateScramSecretOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--secret-arn-list")] string[] SecretArnList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}