namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    [Arguments("agent-identity auth-providers create", "ApiKey")]
    [Arguments("apihub apis versions specs create", "SpecTypeEnumValues")]
    public async Task Captured_Required_Choice_Introductions_Keep_Their_Requiredness(string path, string property)
    {
        var command = await GcloudCapturedSemanticsTests.Scrape(path);
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains(property));
        await Assert.That(group.IsRequired).IsTrue();
        await Assert.That(group.IsMutuallyExclusive).IsTrue();
    }

    [Test]
    [Arguments("agent-identity auth-providers create", "ApiKey", "Message describing ApiKeyParams object.", "Message describing ThreeLeggedOAuth object.")]
    [Arguments("agent-identity auth-providers create", "ThreeLeggedOauthAuthorizationUrl", "Message describing ThreeLeggedOAuth object.", "Message describing ApiKeyParams object.")]
    [Arguments("agent-identity auth-providers create", "TwoLeggedOauthTokenUrl", "Message describing TwoLeggedOAuth object.", "Message describing ThreeLeggedOAuth object.")]
    [Arguments("apihub apis versions specs create", "SpecTypeEnumValues", "The attribute values of data type enum.", "The attribute values of data type string or JSON.")]
    [Arguments("apihub apis versions specs create", "SpecTypeJsonValues", "The attribute values of data type string or JSON.", "The attribute values of data type enum.")]
    [Arguments("apihub apis versions specs create", "SpecTypeStringValues", "The attribute values of data type string or JSON.", "The attribute values of data type enum.")]
    [Arguments("apihub apis versions specs create", "SpecTypeUriValues", "The attribute values of data type string or JSON.", "The attribute values of data type enum.")]
    [Arguments("storage batch-operations jobs create", "TargetProject", "Use a project as the source.", "Use bucket(s) as the source.")]
    [Arguments("storage batch-operations jobs create", "Bucket", "Use bucket(s) as the source.", "Use a project as the source.")]
    [Arguments("oracle-database goldengate connections create", "GoogleBigQueryConnectionPropertiesServiceAccountKeyFile", "The properties of GoldengateGoogleBigQueryConnectionProperties.", "The properties of GoldengateGoogleCloudStorageConnectionProperties.")]
    [Arguments("oracle-database goldengate connections create", "GoogleCloudStorageConnectionPropertiesServiceAccountKeyFile", "The properties of GoldengateGoogleCloudStorageConnectionProperties.", "The properties of GoldengateGoogleBigQueryConnectionProperties.")]
    [Arguments("oracle-database goldengate connections create", "MysqlConnectionPropertiesPasswordSecretVersion", "Properties of GoldengateMysqlConnection.", "The properties of GoldengateGoogleBigQueryConnectionProperties.")]
    [Arguments("developer-connect connections create", "Labels", "Labels as key value pairs.", "The git proxy configuration.")]
    [Arguments("developer-connect connections create", "Secret", "For resources", "The git proxy configuration.")]
    public async Task Captured_Nested_Choice_Descriptions_Stay_With_Their_Own_Branch(
        string path, string property, string ownDescription, string siblingDescription)
    {
        var command = await GcloudCapturedSemanticsTests.Scrape(path);
        var description = command.Options.Single(option => option.PropertyName == property).Description;
        await Assert.That(description).Contains(ownDescription);
        await Assert.That(description).DoesNotContain(siblingDescription);
    }
}
