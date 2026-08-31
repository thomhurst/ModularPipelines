using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudProjectSelectorTests
{
    [Test]
    public async Task CustomModuleSelectorsRenderProjectIdsAndResourceNames()
    {
        const string module = "custom-module";
        const string project = "my-project";
        const string parent = "projects/my-project";
        var options = new object[]
        {
            new GcloudSccManageCustomModulesEtdDeleteOptions(module)
            {
                Project = project,
                Parent = parent,
            },
            new GcloudSccManageCustomModulesEtdDescribeOptions(module)
            {
                Project = project,
                Parent = parent,
            },
            new GcloudSccManageCustomModulesEtdDescribeEffectiveOptions(module)
            {
                Project = project,
                Parent = parent,
            },
            new GcloudSccManageCustomModulesShaDeleteOptions(module)
            {
                Project = project,
                Parent = parent,
            },
            new GcloudSccManageCustomModulesShaDescribeOptions(module)
            {
                Project = project,
                Parent = parent,
            },
            new GcloudSccManageCustomModulesShaDescribeEffectiveOptions(module)
            {
                Project = project,
                Parent = parent,
            },
        };

        foreach (var option in options)
        {
            await Assert.That(BuildArguments(option)).IsEquivalentTo(
            [
                module,
                $"--project={project}",
                $"--parent={parent}",
            ]);
        }
    }
}
