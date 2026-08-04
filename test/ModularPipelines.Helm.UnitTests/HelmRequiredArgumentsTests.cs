using ModularPipelines.Helm.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Helm.UnitTests;

public class HelmRequiredArgumentsTests
{
    [Test]
    public async Task Install_Rejects_Null_Chart()
    {
        var options = new HelmInstallOptions(null!);

        var exception = Assert.Throws<ArgumentException>(() => BuildArguments(options));

        using (Assert.Multiple())
        {
            await Assert.That(exception.ParamName).IsEqualTo(nameof(options.Chart));
            await Assert.That(exception.Message).Contains("HelmInstallOptions.Chart");
        }
    }

    [Test]
    public async Task Install_Allows_Null_Conditional_Name()
    {
        var options = new HelmInstallOptions("repository/chart")
        {
            GenerateName = true,
            Name = null,
        };

        var arguments = BuildArguments(options);

        await Assert.That(arguments).IsEquivalentTo(["repository/chart", "--generate-name"]);
    }
}
