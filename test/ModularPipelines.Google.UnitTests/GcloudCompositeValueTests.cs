using ModularPipelines.Google.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudCompositeValueTests
{
    [Test]
    public async Task AllowedClientsRenderAsRepeatedStructuredValues()
    {
        var arguments = BuildArguments(new GcloudBmsNfsSharesUpdateOptions("nfs-share")
        {
            AddAllowedClient =
            [
                "network=network-a,cidr=10.0.0.0/24,mount-permissions=READ_ONLY",
                "network=network-b,cidr=10.0.1.0/24,mount-permissions=READ_WRITE",
            ],
        });

        await AssertArguments(arguments,
        [
            "nfs-share",
            "--add-allowed-client=network=network-a,cidr=10.0.0.0/24,mount-permissions=READ_ONLY",
            "--add-allowed-client=network=network-b,cidr=10.0.1.0/24,mount-permissions=READ_WRITE",
        ]);
    }

    [Test]
    public async Task UpdateLabelsRenderAsStructuredValues()
    {
        var arguments = BuildArguments(new GcloudBmsNfsSharesUpdateOptions("nfs-share")
        {
            UpdateLabels = [new KeyValue("access", "READ_ONLY")],
        });

        await AssertArguments(arguments, ["nfs-share", "--update-labels=access=READ_ONLY"]);
    }

    [Test]
    public async Task GeminiToolsRenderAsRepeatedStructuredValues()
    {
        var arguments = BuildArguments(new GcloudGeminiCodeToolsSettingsUpdateOptions("code-tools-setting")
        {
            EnabledTool = ["handle=search,tool=projects/p/locations/l/tools/search"],
            AddEnabledTool = ["handle=build,config=[{key=mode,value=fast}]"],
            RemoveEnabledTool = ["handle=legacy,tool=projects/p/locations/l/tools/legacy"],
        });

        await AssertArguments(arguments,
        [
            "code-tools-setting",
            "--enabled-tool=handle=search,tool=projects/p/locations/l/tools/search",
            "--add-enabled-tool=handle=build,config=[{key=mode,value=fast}]",
            "--remove-enabled-tool=handle=legacy,tool=projects/p/locations/l/tools/legacy",
        ]);
    }
}
