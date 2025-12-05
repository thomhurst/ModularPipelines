using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-user-proficiencies")]
public record AwsConnectUpdateUserProficienciesOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--user-proficiencies")] string[] UserProficiencies
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}