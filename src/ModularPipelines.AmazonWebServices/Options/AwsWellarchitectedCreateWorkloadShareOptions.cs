using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "create-workload-share")]
public record AwsWellarchitectedCreateWorkloadShareOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--shared-with")] string SharedWith,
[property: CommandSwitch("--permission-type")] string PermissionType
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}