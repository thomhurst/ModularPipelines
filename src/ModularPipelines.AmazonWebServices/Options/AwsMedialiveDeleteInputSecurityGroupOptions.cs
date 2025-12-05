using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "delete-input-security-group")]
public record AwsMedialiveDeleteInputSecurityGroupOptions(
[property: CliOption("--input-security-group-id")] string InputSecurityGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}