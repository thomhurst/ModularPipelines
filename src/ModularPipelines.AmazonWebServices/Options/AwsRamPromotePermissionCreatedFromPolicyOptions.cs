using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "promote-permission-created-from-policy")]
public record AwsRamPromotePermissionCreatedFromPolicyOptions(
[property: CommandSwitch("--permission-arn")] string PermissionArn,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}