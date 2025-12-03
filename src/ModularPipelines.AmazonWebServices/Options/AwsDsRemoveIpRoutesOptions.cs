using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "remove-ip-routes")]
public record AwsDsRemoveIpRoutesOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--cidr-ips")] string[] CidrIps
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}