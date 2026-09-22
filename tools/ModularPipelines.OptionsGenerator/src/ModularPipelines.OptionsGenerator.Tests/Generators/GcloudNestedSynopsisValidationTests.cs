using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Gcloud_Composer_Updates_Preserve_Optional_Resource_Settings()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("composer environments update");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("MaxWorkers"));
        await ValidateCapturedGroup(command, group,
        [
            ("", false),
            ("MaxWorkers", true),
            ("SchedulerCount", true),
            ("MaxWorkers,SchedulerCount", true),
            ("NodeCount", true),
            ("NodeCount,MaxWorkers", false),
            ("MaintenanceWindowStart,MaintenanceWindowEnd,MaintenanceWindowRecurrence", true),
            ("MaintenanceWindowStart", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Container_Autoprovisioning_Preserves_Optional_Settings()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("container clusters update");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("AutoprovisioningConfigFile"));
        await ValidateCapturedGroup(command, group,
        [
            ("AutoprovisioningConfigFile", true),
            ("EnableAutoprovisioning,AutoprovisioningConfigFile", true),
            ("AutoprovisioningConfigFile,AutoprovisioningMinCpuPlatform", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Cluster_Director_Updates_Do_Not_Require_Unrelated_Granular_Flags()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("cluster-director clusters update");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("Config"));
        await ValidateCapturedGroup(command, group,
        [
            ("", false),
            ("Description", true),
            ("RemoveLabels", true),
            ("Description,RemoveLabels", true),
            ("Config,UpdateMask", true),
            ("Config", false),
            ("UpdateMask", false),
            ("Config,UpdateMask,Description", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Build_Trigger_Updates_Preserve_Documented_Nested_Choices()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "585.0.0",
            "gcloud-builds-triggers-update-github.txt"));
        var command = (await new GcloudResourceArgumentTests.TestScraper().Parse(["gcloud", "builds", "triggers", "update", "github"], help))!;
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("TriggerConfig"));
        await ValidateCapturedGroup(command, group,
        [
            ("", false),
            ("TriggerConfig", true),
            ("BranchPattern", true),
            ("TagPattern", true),
            ("BuildConfig", true),
            ("UpdateSubstitutions", true),
            ("Description", true),
            ("Description,BranchPattern,BuildConfig", true),
            ("TriggerConfig,BranchPattern", false),
            ("BranchPattern,TagPattern", false),
            ("BuildConfig,InlineConfig", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Storage_Source_Branches_Keep_Bucket_And_Filter_Choices_Together()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("storage batch-operations jobs create");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("Bucket"));
        await ValidateCapturedGroup(command, group,
        [
            ("", false),
            ("Bucket,ManifestLocation", true),
            ("BucketList,ManifestLocation", true),
            ("Bucket,IncludedObjectPrefixes", true),
            ("BucketList,IncludedObjectPrefixes", true),
            ("Bucket", false),
            ("ManifestLocation", false),
            ("Bucket,BucketList,ManifestLocation", false),
            ("Bucket,ManifestLocation,IncludedObjectPrefixes", false),
            ("DryRunJobId", true),
            ("InsightsDataSetConfig,TargetProject", true),
            ("InsightsDataSetConfig", false),
            ("TargetProject", false),
            ("InsightsDataSetConfig,TargetProject,TargetLocations,TargetSnapshotTime", true),
            ("Bucket,ManifestLocation,DryRunJobId", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Storage_Custom_Context_Alternatives_Preserve_Documented_Choices()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("storage batch-operations jobs create");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("ClearAllObjectCustomContexts"));
        await ValidateCapturedGroup(command, group,
        [
            ("", false),
            ("ClearAllObjectCustomContexts", true),
            ("ClearObjectCustomContexts,UpdateObjectCustomContexts", true),
            ("ClearObjectCustomContexts", true),
            ("UpdateObjectCustomContexts", true),
            ("ClearObjectCustomContexts,UpdateObjectCustomContextsFile", true),
            ("UpdateObjectCustomContexts,UpdateObjectCustomContextsFile", false),
            ("UpdateObjectCustomContextsFile", true),
            ("ClearAllObjectCustomContexts,UpdateObjectCustomContextsFile", false),
            ("DeleteObject", true),
            ("DeleteObject,EnablePermanentObjectDeletion", true),
            ("EnablePermanentObjectDeletion", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Agent_Identity_Oauth_Preserves_Optional_Members_Within_Exclusive_Branches()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("agent-identity auth-providers create");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("ApiKey"));
        const string threeLegged = "ThreeLeggedOauthAuthorizationUrl,ThreeLeggedOauthClientId,ThreeLeggedOauthClientSecret,ThreeLeggedOauthDefaultContinueUri,ThreeLeggedOauthEnablePkce,ThreeLeggedOauthTokenUrl";
        const string twoLegged = "TwoLeggedOauthClientId,TwoLeggedOauthClientSecret,TwoLeggedOauthTokenUrl";
        var cases = new List<(string Properties, bool Valid)>
        {
            ("", false),
            ("ApiKey", true),
            (threeLegged, true),
            (twoLegged, true),
            ("ApiKey," + twoLegged, false),
        };
        foreach (var branch in new[] { threeLegged, twoLegged })
        {
            var members = branch.Split(',');
            cases.AddRange(members.Select(missing => (string.Join(',', members.Where(member => member != missing)), true)));
            cases.AddRange(members.Select(member => (member, true)));
        }

        await ValidateCapturedGroup(command, group, [.. cases]);
    }
}
