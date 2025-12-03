using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "disable-user")]
public record AwsAppstreamDisableUserOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--authentication-type")] string AuthenticationType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}