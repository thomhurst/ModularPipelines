using ModularPipelines.Podman.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Podman.UnitTests;

public class PodmanComposeOptionsTests
{
    [Test]
    public async Task Compose_Cp_Renders_Source_And_Destination_In_Order()
    {
        var arguments = BuildArguments(new PodmanComposeCpOptions(
            "api:/tmp/report.txt",
            "./report.txt"));

        await AssertArguments(arguments, ["api:/tmp/report.txt", "./report.txt"]);
    }

    [Test]
    public async Task Compose_Cp_Rejects_Missing_Source()
    {
        var options = new PodmanComposeCpOptions(null!, "./report.txt");

        var exception = Assert.Throws<ArgumentException>(() => BuildArguments(options));

        await Assert.That(exception.ParamName).IsEqualTo(nameof(options.ServiceSrcPath));
    }
}
