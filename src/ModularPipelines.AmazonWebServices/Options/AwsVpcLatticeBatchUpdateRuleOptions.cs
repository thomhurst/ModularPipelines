using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "batch-update-rule")]
public record AwsVpcLatticeBatchUpdateRuleOptions(
[property: CliOption("--listener-identifier")] string ListenerIdentifier,
[property: CliOption("--rules")] string[] Rules,
[property: CliOption("--service-identifier")] string ServiceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}