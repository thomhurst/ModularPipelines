using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-instance-profile")]
public record AwsIamCreateInstanceProfileOptions(
[property: CliOption("--instance-profile-name")] string InstanceProfileName
) : AwsOptions
{
    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}