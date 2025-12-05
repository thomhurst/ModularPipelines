using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "shared-vpc", "enable")]
public record GcloudComputeSharedVpcEnableOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;