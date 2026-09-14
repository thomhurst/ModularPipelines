using System.Reflection;
using ModularPipelines.Azure.Options;
using ModularPipelines.Secrets;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Azure.UnitTests;

public class AzureCredentialContractTests
{
    [Test]
    [Arguments(typeof(AzSqlVmAddToGroupOptions), "BootstrapAccPwd")]
    [Arguments(typeof(AzSqlVmAddToGroupOptions), "OperatorAccPwd")]
    [Arguments(typeof(AzSqlVmAddToGroupOptions), "ServiceAccPwd")]
    [Arguments(typeof(AzSqlVmCreateOptions), "BackupPwd")]
    [Arguments(typeof(AzSqlVmUpdateOptions), "BackupPwd")]
    public async Task Sql_Password_Values_Are_Registered_As_Secrets(Type optionsType, string propertyName)
    {
        var property = optionsType.GetProperty(propertyName)!;
        await Assert.That(property.PropertyType).IsEqualTo(typeof(string));
        await Assert.That(property.GetCustomAttribute<SecretValueAttribute>()).IsNotNull();
    }

    [Test]
    [Arguments(typeof(AzVmCreateOptions))]
    [Arguments(typeof(AzVmssCreateOptions))]
    public async Task Admin_Username_Renders_As_One_Value(Type optionsType)
    {
        var options = Activator.CreateInstance(optionsType, ["machine", "group"])!;
        var username = optionsType.GetProperty("AdminUsername")!;
        await Assert.That(username.PropertyType).IsEqualTo(typeof(string));
        username.SetValue(options, "alice");
        await AssertArguments(BuildArguments(options), ["--name", "machine", "--resource-group", "group", "--admin-username", "alice"]);
    }
}
