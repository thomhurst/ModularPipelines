using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "replace-iam-instance-profile-association")]
public record AwsEc2ReplaceIamInstanceProfileAssociationOptions(
[property: CliOption("--iam-instance-profile")] string IamInstanceProfile,
[property: CliOption("--association-id")] string AssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}