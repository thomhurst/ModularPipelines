using ModularPipelines.Google.Enums;
using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudCompositeValueTests
{
    [Test]
    public async Task AllowedClientsRenderAsRepeatedStructuredValues()
    {
        var arguments = BuildArguments(new GcloudBmsNfsSharesUpdateOptions
        {
            AddAllowedClient =
            [
                "network=network-a,cidr=10.0.0.0/24,mount-permissions=READ_ONLY",
                "network=network-b,cidr=10.0.1.0/24,mount-permissions=READ_WRITE",
            ],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--add-allowed-client=network=network-a,cidr=10.0.0.0/24,mount-permissions=READ_ONLY",
            "--add-allowed-client=network=network-b,cidr=10.0.1.0/24,mount-permissions=READ_WRITE",
        ], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task ShippedUpdateLabelsEnumStillRenders()
    {
        var arguments = BuildArguments(new GcloudBmsNfsSharesUpdateOptions
        {
            UpdateLabels = GcloudUpdateLabels.ReadOnly,
        });

        await Assert.That(arguments).IsEquivalentTo(["--update-labels=READ_ONLY"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task GeminiToolsRenderAsRepeatedStructuredValues()
    {
        var arguments = BuildArguments(new GcloudGeminiCodeToolsSettingsUpdateOptions
        {
            EnabledTool = ["handle=search,tool=projects/p/locations/l/tools/search"],
            AddEnabledTool = ["handle=build,config=[{key=mode,value=fast}]"],
            RemoveEnabledTool = ["handle=legacy,tool=projects/p/locations/l/tools/legacy"],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--enabled-tool=handle=search,tool=projects/p/locations/l/tools/search",
            "--add-enabled-tool=handle=build,config=[{key=mode,value=fast}]",
            "--remove-enabled-tool=handle=legacy,tool=projects/p/locations/l/tools/legacy",
        ], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }
}
