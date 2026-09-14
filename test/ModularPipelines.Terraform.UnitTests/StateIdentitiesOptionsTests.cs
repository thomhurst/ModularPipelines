using ModularPipelines.Context;
using ModularPipelines.Terraform.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Terraform.UnitTests;

public class StateIdentitiesOptionsTests : TestBase
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Json_Flag_Precedes_Optional_Address_Filters(bool filterAddresses)
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new TerraformStateIdentitiesOptions(true)
        {
            Address = filterAddresses ? ["aws_instance.example", "module.example"] : null,
        };

        var commandLine = builder.Build(options);

        await Assert.That(commandLine.ToString()).IsEqualTo(filterAddresses
            ? "terraform state identities -json aws_instance.example module.example"
            : "terraform state identities -json");
    }
}
