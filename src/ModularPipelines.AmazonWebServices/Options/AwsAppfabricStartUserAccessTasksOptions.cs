using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "start-user-access-tasks")]
public record AwsAppfabricStartUserAccessTasksOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CommandSwitch("--email")] string Email
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}