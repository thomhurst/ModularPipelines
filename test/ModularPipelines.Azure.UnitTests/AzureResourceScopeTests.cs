using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using ModularPipelines.Azure.Exceptions;
using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Provisioning.Security;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.UnitTests;

public class AzureResourceScopeTests
{
    private const string SubscriptionId = "00000000-0000-0000-0000-000000000001";
    private const string ResourceGroup = "test-group";

    [Test]
    public async Task Resolves_Type_And_Name_Case_Insensitively_Across_Pages()
    {
        var plan = Resource("Microsoft.Web/serverfarms", "shared-name");
        var site = Resource("Microsoft.Web/sites", "SHARED-NAME");
        var client = new TestArmClient(plan, site);
        var scope = new AzureResourceIdentifier(SubscriptionId, ResourceGroup, "microsoft.web/SITES", "shared-name");

        var result = await client.GetResourceIdentifierAsync(scope);

        await Assert.That(result).IsEqualTo(site.Id);
    }

    [Test]
    public async Task ResourceIdentifier_Conversion_Preserves_Type()
    {
        var resource = Resource("Microsoft.Web/sites", "site");
        AzureResourceIdentifier scope = resource.Id;

        await Assert.That(scope.SubscriptionId).IsEqualTo(SubscriptionId);
        await Assert.That(scope.ResourceGroupName).IsEqualTo(ResourceGroup);
        await Assert.That(scope.ResourceType).IsEqualTo("Microsoft.Web/sites");
        await Assert.That(scope.ResourceName).IsEqualTo("site");
        await Assert.That(await new TestArmClient(resource).GetResourceIdentifierAsync(scope)).IsEqualTo(resource.Id);
    }

    [Test]
    public async Task Factories_Require_And_Preserve_Resource_Type()
    {
        var expected = new AzureResourceIdentifier(SubscriptionId, ResourceGroup, "Microsoft.Web/sites", "site");

        await Assert.That(new AzureSubscriptionIdentifier(SubscriptionId)
            .GetResourceIdentifier(ResourceGroup, "Microsoft.Web/sites", "site")).IsEqualTo(expected);
        await Assert.That(new AzureResourceGroupIdentifier(SubscriptionId, ResourceGroup)
            .GetResourceIdentifier("Microsoft.Web/sites", "site")).IsEqualTo(expected);
    }

    [Test]
    [Arguments("Microsoft.Web/serverfarms", "shared-name")]
    [Arguments("Microsoft.Web/sites", "different-name")]
    public async Task Missing_Resource_Reports_Requested_Scope(string resourceType, string resourceName)
    {
        var scope = new AzureResourceIdentifier(SubscriptionId, ResourceGroup, "Microsoft.Web/sites", "shared-name");
        var client = new TestArmClient(Resource(resourceType, resourceName));

        var exception = await Assert.That(async () => { await client.GetResourceIdentifierAsync(scope); })
            .Throws<AzureResourceNotFoundException>();

        await Assert.That(exception!.ResourceIdentifier).IsEqualTo(scope);
    }

    [Test]
    public async Task Unsupported_Scope_Reports_Requested_Scope()
    {
        var scope = new UnsupportedScope();
        var client = new TestArmClient();

        var exception = await Assert.That(async () => { await client.GetResourceIdentifierAsync(scope); })
            .Throws<UnsupportedAzureScopeException>();

        await Assert.That(exception!.Scope).IsEqualTo(scope);
        await Assert.That(client.GroupRequested).IsFalse();
    }

    [Test]
    public async Task Non_Resource_Scopes_Do_Not_List_Resources()
    {
        var client = new TestArmClient();
        var subscription = new AzureSubscriptionIdentifier(SubscriptionId);
        var group = new AzureResourceGroupIdentifier(SubscriptionId, ResourceGroup);
        var management = new AzureManagementGroupIdentifier("management");

        await Assert.That(await client.GetResourceIdentifierAsync(subscription)).IsEqualTo(subscription.ToSubscriptionIdentifier());
        await Assert.That(await client.GetResourceIdentifierAsync(group)).IsEqualTo(group.ToResourceGroupIdentifier());
        await Assert.That(await client.GetResourceIdentifierAsync(management)).IsEqualTo(management.ToManagementGroupIdentifier());
        await Assert.That(client.GroupRequested).IsFalse();
    }

    [Test]
    public async Task Lookup_Propagates_Cancellation_To_Listing_And_Enumeration()
    {
        using var cancellation = new CancellationTokenSource();
        var client = new TestArmClient();
        client.Group.Resources.BeforeEnumeration = cancellation.Cancel;
        var scope = new AzureResourceIdentifier(SubscriptionId, ResourceGroup, "Microsoft.Web/sites", "site");

        await Assert.That(async () => { await client.GetResourceIdentifierAsync(scope, cancellation.Token); }).Throws<OperationCanceledException>();

        await Assert.That(client.Group.ListingToken).IsEqualTo(cancellation.Token);
        await Assert.That(client.Group.Resources.EnumerationToken).IsEqualTo(cancellation.Token);
    }

    [Test]
    public async Task Precancelled_Lookup_Does_Not_List_Resources()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var client = new TestArmClient();
        var scope = new AzureResourceIdentifier(SubscriptionId, ResourceGroup, "Microsoft.Web/sites", "site");

        await Assert.That(async () => { await client.GetResourceIdentifierAsync(scope, cancellation.Token); }).Throws<OperationCanceledException>();
        await Assert.That(client.GroupRequested).IsFalse();
    }

    [Test]
    public async Task Role_Definition_Propagates_Cancellation_To_Scope_Resolution()
    {
        using var cancellation = new CancellationTokenSource();
        var client = new TestArmClient();
        client.Group.Resources.BeforeEnumeration = cancellation.Cancel;
        var provisioner = new AzureSecurityProvisioner(client);
        var scope = new AzureResourceIdentifier(SubscriptionId, ResourceGroup, "Microsoft.Web/sites", "site");
        var definition = new ResourceIdentifier($"/subscriptions/{SubscriptionId}/providers/Microsoft.Authorization/roleDefinitions/00000000-0000-0000-0000-000000000002");

        await Assert.That(async () => { await provisioner.AuthorizationRoleDefinitionAsync(scope, definition, new(), cancellation.Token); })
            .Throws<OperationCanceledException>();
        await Assert.That(client.Group.ListingToken).IsEqualTo(cancellation.Token);
        await Assert.That(client.Group.Resources.EnumerationToken).IsEqualTo(cancellation.Token);
    }

    private sealed record UnsupportedScope : AzureScope;

    private static GenericResource Resource(string type, string name) =>
        new TestResource(new ResourceIdentifier($"/subscriptions/{SubscriptionId}/resourceGroups/{ResourceGroup}/providers/{type}/{name}"));

    private sealed class TestResource(ResourceIdentifier id) : GenericResource
    {
        public override ResourceIdentifier Id => id;
    }

    private sealed class TestArmClient(params GenericResource[] resources) : ArmClient
    {
        public TestResourceGroup Group { get; } = new(resources);

        public bool GroupRequested { get; private set; }

        public override ResourceGroupResource GetResourceGroupResource(ResourceIdentifier id)
        {
            GroupRequested = true;
            return Group;
        }
    }

    private sealed class TestResourceGroup(params GenericResource[] resources) : ResourceGroupResource
    {
        public TestResources Resources { get; } = new(resources);

        public CancellationToken ListingToken { get; private set; }

        public override AsyncPageable<GenericResource> GetGenericResourcesAsync(string? filter = null, string? expand = null, int? top = null, CancellationToken cancellationToken = default)
        {
            ListingToken = cancellationToken;
            return Resources;
        }
    }

    private sealed class TestResources(params GenericResource[] resources) : AsyncPageable<GenericResource>
    {
        public Action? BeforeEnumeration { get; set; }

        public CancellationToken EnumerationToken { get; private set; }

        public override async IAsyncEnumerator<GenericResource> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            EnumerationToken = cancellationToken;
            BeforeEnumeration?.Invoke();
            cancellationToken.ThrowIfCancellationRequested();
            await foreach (var page in AsPages())
            {
                foreach (var resource in page.Values)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return resource;
                }
            }
        }

        public override async IAsyncEnumerable<Page<GenericResource>> AsPages(string? continuationToken = null, int? pageSizeHint = null)
        {
            await Task.CompletedTask;
            foreach (var resource in resources)
            {
                yield return Page<GenericResource>.FromValues([resource], null, null!);
            }
        }
    }
}
