using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "install-applications")]
public record AwsEmrInstallApplicationsOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--applications")] string[] Applications
) : AwsOptions;