using System.ComponentModel.DataAnnotations;
using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudEmptyResetTests
{
    [Test]
    [Arguments(null, false)]
    [Arguments("", true)]
    [Arguments("DEFAULT", true)]
    [Arguments(" \t", false)]
    public async Task Kafka_Mapping_Reset_Counts_As_An_Update(string? rules, bool valid)
    {
        var options = new GcloudManagedKafkaClustersUpdateOptions("cluster")
        {
            SslPrincipalMappingRules = rules,
        };
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    public async Task Kafka_Empty_Reset_Renders_An_Explicit_Empty_Value()
    {
        var options = new GcloudManagedKafkaClustersUpdateOptions("cluster")
        {
            SslPrincipalMappingRules = "",
        };

        await AssertArguments(BuildArguments(options), ["cluster", "--ssl-principal-mapping-rules="]);
    }

    [Test]
    public async Task Kafka_Empty_Ordinary_Option_Does_Not_Count_As_An_Update()
    {
        var options = new GcloudManagedKafkaClustersUpdateOptions("cluster") { Cpu = "" };

        await Assert.That(Validator.TryValidateObject(options, new(options), [], true)).IsFalse();
    }

    [Test]
    [Arguments(null, null, null, true)]
    [Arguments("", null, null, true)]
    [Arguments("", "03:00", null, true)]
    [Arguments(null, "03:00", null, true)]
    [Arguments("europe-west1", "03:00", null, true)]
    [Arguments(null, null, true, true)]
    [Arguments("", null, true, false)]
    [Arguments("", "03:00", true, false)]
    public async Task Sql_Backup_Reset_Remains_Exclusive_With_NoBackup(
        string? location, string? startTime, bool? noBackup, bool valid)
    {
        var options = new GcloudSqlInstancesPatchOptions("instance")
        {
            BackupLocation = location,
            BackupStartTime = startTime,
            NoBackup = noBackup,
        };
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    public async Task Sql_Backup_Reset_Renders_An_Explicit_Empty_Value()
    {
        var options = new GcloudSqlInstancesPatchOptions("instance") { BackupLocation = "" };

        await AssertArguments(BuildArguments(options), ["instance", "--backup-location="]);
    }
}
