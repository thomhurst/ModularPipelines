using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "policycontroller")]
public class GcloudContainerHubPolicycontrollerContent
{
    public GcloudContainerHubPolicycontrollerContent(
        GcloudContainerHubPolicycontrollerContentBundles bundles,
        GcloudContainerHubPolicycontrollerContentTemplates templates
    )
    {
        Bundles = bundles;
        Templates = templates;
    }

    public GcloudContainerHubPolicycontrollerContentBundles Bundles { get; }

    public GcloudContainerHubPolicycontrollerContentTemplates Templates { get; }
}