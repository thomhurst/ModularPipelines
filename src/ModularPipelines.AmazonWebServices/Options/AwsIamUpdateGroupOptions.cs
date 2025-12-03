using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-group")]
public record AwsIamUpdateGroupOptions(
[property: CliOption("--group-name")] string GroupName
) : AwsOptions
{
    [CliOption("--new-path")]
    public string? NewPath { get; set; }

    [CliOption("--new-group-name")]
    public string? NewGroupName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}