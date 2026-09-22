using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
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
    public async Task Gcloud_Storage_Custom_Context_Alternatives_Preserve_Required_Pairs()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("storage batch-operations jobs create");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("ClearAllObjectCustomContexts"));
        await ValidateCapturedGroup(command, group,
        [
            ("", false),
            ("ClearAllObjectCustomContexts", true),
            ("ClearObjectCustomContexts,UpdateObjectCustomContexts", true),
            ("ClearObjectCustomContexts", false),
            ("UpdateObjectCustomContexts", false),
            ("UpdateObjectCustomContextsFile", true),
            ("ClearAllObjectCustomContexts,UpdateObjectCustomContextsFile", false),
            ("DeleteObject", true),
            ("DeleteObject,EnablePermanentObjectDeletion", true),
            ("EnablePermanentObjectDeletion", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Agent_Identity_Oauth_Requires_Every_Selected_Branch_Member()
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
            cases.AddRange(members.Select(missing => (string.Join(',', members.Where(member => member != missing)), false)));
        }

        await ValidateCapturedGroup(command, group, [.. cases]);
    }
}
