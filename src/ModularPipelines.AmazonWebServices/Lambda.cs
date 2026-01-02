using System.Diagnostics.CodeAnalysis;
using Amazon.Lambda;
using Amazon.Lambda.Model;

namespace ModularPipelines.AmazonWebServices;

/// <summary>
/// Provides wrapper methods for AWS Lambda operations.
/// </summary>
[ExcludeFromCodeCoverage]
public class Lambda
{
    /// <summary>
    /// Gets the underlying AWS Lambda client.
    /// </summary>
    public AmazonLambdaClient Client { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Lambda"/> class.
    /// </summary>
    /// <param name="client">The AWS Lambda client.</param>
    public Lambda(AmazonLambdaClient client)
    {
        Client = client;
    }

    /// <summary>
    /// Invokes a Lambda function synchronously or asynchronously based on the request configuration.
    /// </summary>
    /// <param name="request">The invoke request containing function name and payload.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response from the Lambda function invocation.</returns>
    public async Task<InvokeResponse> Invoke(InvokeRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.InvokeAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists all Lambda functions in the AWS account.
    /// </summary>
    /// <param name="request">The request containing optional filters and pagination options.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A response containing the list of Lambda functions.</returns>
    public async Task<ListFunctionsResponse> ListFunctions(ListFunctionsRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.ListFunctionsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves information about a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name or ARN.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The function configuration and code details.</returns>
    public async Task<GetFunctionResponse> GetFunction(GetFunctionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetFunctionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves the configuration of a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name or ARN.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The function configuration details.</returns>
    public async Task<GetFunctionConfigurationResponse> GetFunctionConfiguration(GetFunctionConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetFunctionConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a new Lambda function.
    /// </summary>
    /// <param name="request">The request containing function configuration and code.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The created function configuration.</returns>
    public async Task<CreateFunctionResponse> CreateFunction(CreateFunctionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.CreateFunctionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name or ARN to delete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the delete operation.</returns>
    public async Task<DeleteFunctionResponse> DeleteFunction(DeleteFunctionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.DeleteFunctionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates a Lambda function's code.
    /// </summary>
    /// <param name="request">The request containing the new code location or content.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The updated function configuration.</returns>
    public async Task<UpdateFunctionCodeResponse> UpdateFunctionCode(UpdateFunctionCodeRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.UpdateFunctionCodeAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates a Lambda function's configuration.
    /// </summary>
    /// <param name="request">The request containing the configuration changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The updated function configuration.</returns>
    public async Task<UpdateFunctionConfigurationResponse> UpdateFunctionConfiguration(UpdateFunctionConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.UpdateFunctionConfigurationAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Publishes a version of a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name and optional description.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The published function version configuration.</returns>
    public async Task<PublishVersionResponse> PublishVersion(PublishVersionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PublishVersionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists published versions of a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A response containing the list of function versions.</returns>
    public async Task<ListVersionsByFunctionResponse> ListVersionsByFunction(ListVersionsByFunctionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.ListVersionsByFunctionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates or updates an alias for a Lambda function version.
    /// </summary>
    /// <param name="request">The request containing the alias configuration.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The created alias configuration.</returns>
    public async Task<CreateAliasResponse> CreateAlias(CreateAliasRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.CreateAliasAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates a Lambda function alias.
    /// </summary>
    /// <param name="request">The request containing the alias updates.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The updated alias configuration.</returns>
    public async Task<UpdateAliasResponse> UpdateAlias(UpdateAliasRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.UpdateAliasAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a Lambda function alias.
    /// </summary>
    /// <param name="request">The request containing the alias to delete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the delete operation.</returns>
    public async Task<DeleteAliasResponse> DeleteAlias(DeleteAliasRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.DeleteAliasAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets information about a Lambda function alias.
    /// </summary>
    /// <param name="request">The request containing the function name and alias name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The alias configuration.</returns>
    public async Task<GetAliasResponse> GetAlias(GetAliasRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetAliasAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists aliases for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A response containing the list of aliases.</returns>
    public async Task<ListAliasesResponse> ListAliases(ListAliasesRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.ListAliasesAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds a permission to a Lambda function's resource-based policy.
    /// </summary>
    /// <param name="request">The request containing the permission details.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response containing the statement added to the policy.</returns>
    public async Task<AddPermissionResponse> AddPermission(AddPermissionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.AddPermissionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a permission from a Lambda function's resource-based policy.
    /// </summary>
    /// <param name="request">The request containing the statement ID to remove.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the remove operation.</returns>
    public async Task<RemovePermissionResponse> RemovePermission(RemovePermissionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.RemovePermissionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the resource-based policy for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The function's resource-based policy.</returns>
    public async Task<GetPolicyResponse> GetPolicy(GetPolicyRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetPolicyAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds tags to a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function ARN and tags.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the tag operation.</returns>
    public async Task<TagResourceResponse> TagResource(TagResourceRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.TagResourceAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes tags from a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function ARN and tag keys to remove.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the untag operation.</returns>
    public async Task<UntagResourceResponse> UntagResource(UntagResourceRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.UntagResourceAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists tags for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function ARN.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A response containing the function's tags.</returns>
    public async Task<ListTagsResponse> ListTags(ListTagsRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.ListTagsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates an event source mapping for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the event source configuration.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The created event source mapping configuration.</returns>
    public async Task<CreateEventSourceMappingResponse> CreateEventSourceMapping(CreateEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.CreateEventSourceMappingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates an event source mapping for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the event source mapping updates.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The updated event source mapping configuration.</returns>
    public async Task<UpdateEventSourceMappingResponse> UpdateEventSourceMapping(UpdateEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.UpdateEventSourceMappingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes an event source mapping.
    /// </summary>
    /// <param name="request">The request containing the event source mapping UUID.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The deleted event source mapping configuration.</returns>
    public async Task<DeleteEventSourceMappingResponse> DeleteEventSourceMapping(DeleteEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.DeleteEventSourceMappingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets information about an event source mapping.
    /// </summary>
    /// <param name="request">The request containing the event source mapping UUID.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The event source mapping configuration.</returns>
    public async Task<GetEventSourceMappingResponse> GetEventSourceMapping(GetEventSourceMappingRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetEventSourceMappingAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists event source mappings for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing optional function name filter.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A response containing the list of event source mappings.</returns>
    public async Task<ListEventSourceMappingsResponse> ListEventSourceMappings(ListEventSourceMappingsRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.ListEventSourceMappingsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Configures reserved concurrency for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the concurrency configuration.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The configured concurrency settings.</returns>
    public async Task<PutFunctionConcurrencyResponse> PutFunctionConcurrency(PutFunctionConcurrencyRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PutFunctionConcurrencyAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes reserved concurrency configuration from a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the operation.</returns>
    public async Task<DeleteFunctionConcurrencyResponse> DeleteFunctionConcurrency(DeleteFunctionConcurrencyRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.DeleteFunctionConcurrencyAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the concurrency configuration for a Lambda function.
    /// </summary>
    /// <param name="request">The request containing the function name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The function's concurrency configuration.</returns>
    public async Task<GetFunctionConcurrencyResponse> GetFunctionConcurrency(GetFunctionConcurrencyRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetFunctionConcurrencyAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Creates a Lambda layer version.
    /// </summary>
    /// <param name="request">The request containing the layer configuration.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The created layer version configuration.</returns>
    public async Task<PublishLayerVersionResponse> PublishLayerVersion(PublishLayerVersionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.PublishLayerVersionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Deletes a version of a Lambda layer.
    /// </summary>
    /// <param name="request">The request containing the layer name and version number.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the delete operation.</returns>
    public async Task<DeleteLayerVersionResponse> DeleteLayerVersion(DeleteLayerVersionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.DeleteLayerVersionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets information about a version of a Lambda layer.
    /// </summary>
    /// <param name="request">The request containing the layer name and version number.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The layer version configuration.</returns>
    public async Task<GetLayerVersionResponse> GetLayerVersion(GetLayerVersionRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetLayerVersionAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists Lambda layers.
    /// </summary>
    /// <param name="request">The request containing optional filters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A response containing the list of layers.</returns>
    public async Task<ListLayersResponse> ListLayers(ListLayersRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.ListLayersAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Lists versions of a Lambda layer.
    /// </summary>
    /// <param name="request">The request containing the layer name.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A response containing the list of layer versions.</returns>
    public async Task<ListLayerVersionsResponse> ListLayerVersions(ListLayerVersionsRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.ListLayerVersionsAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the account-level settings for Lambda.
    /// </summary>
    /// <param name="request">The request to get account settings.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The account settings for Lambda.</returns>
    public async Task<GetAccountSettingsResponse> GetAccountSettings(GetAccountSettingsRequest request, CancellationToken cancellationToken = default)
    {
        return await Client.GetAccountSettingsAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
