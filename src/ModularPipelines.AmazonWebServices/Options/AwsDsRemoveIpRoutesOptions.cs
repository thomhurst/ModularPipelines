using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "remove-ip-routes")]
public record AwsDsRemoveIpRoutesOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--cidr-ips")] string[] CidrIps
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}