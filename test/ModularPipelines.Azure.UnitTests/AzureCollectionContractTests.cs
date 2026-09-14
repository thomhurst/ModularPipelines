using ModularPipelines.Azure.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Azure.UnitTests;

public class AzureCollectionContractTests
{
    private static readonly string[] value = new[] { "alice", "bob" };

    [Test]
    [Arguments("Administrators", "--administrators")]
    [Arguments("BackupOperators", "--backup-operators")]
    [Arguments("SecurityOperators", "--security-operators")]
    public async Task Netapp_User_Lists_Render_As_Grouped_Values(string propertyName, string switchName)
    {
        var options = new AzNetappfilesAccountAdAddOptions("account", "group");
        var property = options.GetType().GetProperty(propertyName)!;
        await Assert.That(property.PropertyType).IsEqualTo(typeof(IEnumerable<string>));
        property.SetValue(options, value);

        await AssertArguments(BuildArguments(options),
            ["--account-name", "account", "--resource-group", "group", switchName, "alice", "bob"]);
    }
}
