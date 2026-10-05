using ModularPipelines.Attributes;
using ModularPipelines.Flyway.Options;
using ModularPipelines.Models;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Flyway.UnitTests;

public class FlywayGlobalOptionsTests
{
    [Test]
    public async Task Migration_Inherits_And_Renders_Global_Configuration_And_Maps()
    {
        var options = new FlywayMigrateOptions
        {
            Debug = true,
            JdbcProperties = [new KeyValue("accessToken", "example-token")],
            Locations = "filesystem:a,filesystem:b",
            Password = "example-password",
            Placeholders = [new KeyValue("schema", "public"), new KeyValue("tenant", "blue")],
            Url = "jdbc:h2:mem:test",
            User = "example-user",
        };

        await Assert.That(string.Join(" ", OptionsRenderingTestHelper.BuildArguments(options))).IsEqualTo(
            "-X -jdbcProperties.accessToken=example-token -locations=filesystem:a,filesystem:b -password=example-password -placeholders.schema=public -placeholders.tenant=blue -url=jdbc:h2:mem:test -user=example-user");
        await Assert.That(typeof(FlywayOptions).IsDefined(typeof(CliGlobalOptionsAttribute), inherit: false)).IsTrue();
        await Assert.That(typeof(FlywayMigrateOptions).GetProperty(nameof(FlywayOptions.Password))!
            .IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(typeof(FlywayMigrateOptions).GetProperty(nameof(FlywayOptions.LicenseKey))!
            .IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(typeof(FlywayMigrateOptions).GetProperty(nameof(FlywayOptions.JdbcProperties))!
            .IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }
}
