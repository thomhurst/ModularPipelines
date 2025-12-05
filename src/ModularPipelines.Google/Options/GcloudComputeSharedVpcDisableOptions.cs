using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "shared-vpc", "disable")]
public record GcloudComputeSharedVpcDisableOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;