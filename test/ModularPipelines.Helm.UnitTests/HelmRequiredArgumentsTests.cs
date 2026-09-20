using ModularPipelines.Helm.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Helm.UnitTests;

public class HelmRequiredArgumentsTests
{
    [Test]
    public async Task Install_Rejects_Null_Chart()
    {
        var exception = Assert.Throws<ArgumentException>(() => BuildArguments(new HelmInstallOptions(null!)));
        var expectedDiagnostic = exception is ArgumentNullException
            ? nameof(HelmInstallOptions.Chart)
            : $"{nameof(HelmInstallOptions)}.{nameof(HelmInstallOptions.Chart)}";

        using (Assert.Multiple())
        {
            await Assert.That(exception.ParamName).IsEqualTo(nameof(HelmInstallOptions.Chart));
            await Assert.That(exception.Message).Contains(expectedDiagnostic);
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

        await AssertArguments(arguments, ["repository/chart", "--generate-name"]);
    }
}
