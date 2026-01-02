namespace ModularPipelines.GitHub;

/// <summary>
/// Factory for creating configured HttpMessageHandler instances.
/// </summary>
internal interface IHttpMessageHandlerFactory
{
    /// <summary>
    /// Creates an HttpMessageHandler configured for the specified logical name.
    /// </summary>
    /// <param name="name">The logical name for the handler to create.</param>
    /// <returns>A configured HttpMessageHandler instance.</returns>
    HttpMessageHandler CreateHandler(string name);
}
