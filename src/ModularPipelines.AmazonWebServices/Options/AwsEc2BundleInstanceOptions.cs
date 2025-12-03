using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "bundle-instance")]
public record AwsEc2BundleInstanceOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--storage")]
    public string? Storage { get; set; }

    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--owner-akid")]
    public string? OwnerAkid { get; set; }

    [CliOption("--owner-sak")]
    public string? OwnerSak { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}