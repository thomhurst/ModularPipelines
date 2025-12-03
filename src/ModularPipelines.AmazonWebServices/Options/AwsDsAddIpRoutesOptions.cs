using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "add-ip-routes")]
public record AwsDsAddIpRoutesOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--ip-routes")] string[] IpRoutes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}