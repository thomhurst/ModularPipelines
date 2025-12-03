using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "install-applications")]
public record AwsEmrInstallApplicationsOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--applications")] string[] Applications
) : AwsOptions;