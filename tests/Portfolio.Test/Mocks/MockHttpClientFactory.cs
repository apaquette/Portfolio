using Moq;

namespace Portfolio.Test.Mocks;

public static class MockHttpClientFactory
{
    /// <summary>
    /// Creates a mock HttpClient that returns JSON for a specific URL
    /// </summary>
    public static Mock<HttpClient> CreateMockHttpClient(
        string jsonUrl, 
        string jsonResponse)
    {
        var mockHttp = new Mock<HttpClient>();
        
        mockHttp
            .Setup(http => http.GetStringAsync(It.Is<string>(url => url == jsonUrl)))
            .ReturnsAsync(jsonResponse);
            
        return mockHttp;
    }

    /// <summary>
    /// Creates a mock HttpClient that throws an exception for all requests
    /// </summary>
    public static Mock<HttpClient> CreateFailingHttpClient()
    {
        var mockHttp = new Mock<HttpClient>();
        
        mockHttp
            .Setup(http => http.GetStringAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("Connection failed"));
            
        return mockHttp;
    }

    /// <summary>
    /// Creates a mock HttpClient that returns empty response
    /// </summary>
    public static Mock<HttpClient> CreateEmptyHttpClient()
    {
        var mockHttp = new Mock<HttpClient>();
        
        mockHttp
            .Setup(http => http.GetStringAsync(It.IsAny<string>()))
            .ReturnsAsync(string.Empty);
            
        return mockHttp;
    }
}