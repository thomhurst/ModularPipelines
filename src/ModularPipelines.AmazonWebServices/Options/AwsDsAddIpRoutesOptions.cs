using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "add-ip-routes")]
public record AwsDsAddIpRoutesOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--ip-routes")] string[] IpRoutes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}