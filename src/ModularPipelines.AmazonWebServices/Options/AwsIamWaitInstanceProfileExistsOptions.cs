using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "wait", "instance-profile-exists")]
public record AwsIamWaitInstanceProfileExistsOptions(
[property: CommandSwitch("--instance-profile-name")] string InstanceProfileName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}