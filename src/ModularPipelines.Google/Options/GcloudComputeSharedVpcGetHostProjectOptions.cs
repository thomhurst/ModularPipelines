using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "get-host-project")]
public record GcloudComputeSharedVpcGetHostProjectOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;