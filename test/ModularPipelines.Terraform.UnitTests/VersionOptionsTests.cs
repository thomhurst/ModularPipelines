using ModularPipelines.Context;
using ModularPipelines.Terraform.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Terraform.UnitTests;

public class VersionOptionsTests : TestBase
{
    [Test]
    public async Task Version_Json_Uses_Terraform_Single_Dash_Flag()
    {
        var builder = await GetService<ICommandLineBuilder>();

        await Assert.That(RenderCommand(builder.Build(new TerraformVersionOptions { Json = true })))
            .IsEqualTo("terraform version -json");
        await Assert.That(RenderCommand(builder.Build(new TerraformStacksVersionOptions { Json = true })))
            .IsEqualTo("terraform stacks version -json");
    }
}
