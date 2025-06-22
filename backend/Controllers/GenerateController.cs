using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

// Define a record for the incoming request from the React frontend.
public record GenerateRequest(string ProductName, string Keywords);

[ApiController] // This attribute indicates that this class is an API controller.
[Route("api/[controller]")] // This will make the route /api/generate

// to access this controller.
public class GenerateController : ControllerBase
{
    // We use IHttpClientFactory for efficient management of HttpClient instances.
    private readonly IHttpClientFactory _httpClientFactory;

    // Constructor to inject the IHttpClientFactory dependency.
    public GenerateController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost] // This marks the method to handle POST requests.

    // The method to generate a product description using OpenAI's API.
    public async Task<IActionResult> Post([FromBody] GenerateRequest request)
    {
        // --- THIS IS THE INTENTIONAL VULNERABILITY (Unsecure Version) ---
        // The secret OpenAI API key is hardcoded directly in the source code.
        const string OPENAI_API_KEY = "sk_live_very_secret_api_key_1234567890"; // Replace with your actual OpenAI API key.


        var prompt = $"Write a short, catchy, and professional product description for a \"{request.ProductName}\" that highlights these keywords: \"{request.Keywords}\"."; // Construct the prompt using the product name and keywords from the request.


        var httpClient = _httpClientFactory.CreateClient(); // Create an HttpClient instance using the injected factory.
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OPENAI_API_KEY); // Set the Authorization header with the OpenAI API key.

        // Prepare the request body for OpenAI's API.
        var openAIRequest = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            max_tokens = 100
        };

        // Serialize the request body to JSON format.
        var jsonRequest = JsonSerializer.Serialize(openAIRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"); // Create the content for the HTTP request with the serialized JSON.

        // try to send the request to OpenAI's API and handle the response.
        try
        {
            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content); // Send the POST request to OpenAI's API.

            if (!response.IsSuccessStatusCode) // Check if the response indicates a failure.
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"OpenAI API Error: {errorBody}");
                return StatusCode(500, "Failed to generate description due to an API error.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync(); // Read the response content as a string.

            using (JsonDocument doc = JsonDocument.Parse(jsonResponse)) // Parse the JSON response.
            {
                JsonElement root = doc.RootElement; // Get the root element of the JSON document.
                string description = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()?.Trim() ?? ""; // Extract the generated description from the response.
                return Ok(new { description }); // Return the generated description as a JSON response.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception in GenerateController: {ex.Message}");
            return StatusCode(500, "An internal server error occurred.");
        }
    }
}