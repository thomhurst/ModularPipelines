using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "describe-patch-properties")]
public record AwsSsmDescribePatchPropertiesOptions(
[property: CliOption("--operating-system")] string OperatingSystem,
[property: CliOption("--property")] string Property
) : AwsOptions
{
    [CliOption("--patch-set")]
    public string? PatchSet { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}