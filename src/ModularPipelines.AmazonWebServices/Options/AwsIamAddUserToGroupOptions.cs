using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "add-user-to-group")]
public record AwsIamAddUserToGroupOptions(
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--user-name")] string UserName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}