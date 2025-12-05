using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "shared-vpc", "get-host-project")]
public record GcloudComputeSharedVpcGetHostProjectOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;