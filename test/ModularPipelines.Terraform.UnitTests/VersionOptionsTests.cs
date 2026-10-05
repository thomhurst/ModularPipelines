using ModularPipelines.Context;
using ModularPipelines.Terraform.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Terraform.UnitTests;

public class VersionOptionsTests : TestBase
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Version_Renders_Json_Flag_For_Tool_And_Stacks(bool stacks)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = stacks
            ? builder.Build(new TerraformStacksVersionOptions { Json = true })
            : builder.Build(new TerraformVersionOptions { Json = true });

        await Assert.That(command.ToString()).IsEqualTo(stacks
            ? "terraform stacks version -json"
            : "terraform version -json");
    }
}
