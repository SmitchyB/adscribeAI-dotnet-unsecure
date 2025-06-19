# InputValid-dotnet-unsecure - .NET 8 Vulnerable Build (Insecure Secrets Management)

This repository houses a specific application build that is part of a larger comparative study, "Evaluating the Effectiveness of Secure Coding Practices Across Python, MERN, and .NET 8." The experiment systematically assesses how secure coding techniques mitigate critical web application vulnerabilities—specifically improper input validation, insecure secrets management, and insecure error handling—across these three diverse development stacks. Through the development of paired vulnerable and secure application versions, this study aims to provide empirical evidence on the practical effectiveness of various security controls and the impact of architectural differences on developer effort and overall security posture.

## Purpose
This particular build contains the **Vulnerable Build** of the C# .NET 8 application, specifically designed to demonstrate **Insecure Secrets Management**.

## Vulnerability Focus
This application build explicitly demonstrates:
* **Insecure Secrets Management:** The application contains sensitive information directly embedded within its source code.

## Description of Vulnerability in this Build
In this version, a critical security flaw related to secrets management is intentionally present. The **OpenAI API key** required to access the external AI service is **hardcoded directly within the `GenerateController.cs` file**. This is a highly insecure practice because:
* The secret is exposed in plain text within the version control system.
* Any developer with access to the codebase can immediately view and potentially misuse the API key.
* If the source code is ever leaked (e.g., via public repositories, misconfigured servers), the secret becomes publicly available, leading to potential abuse, financial cost, or unauthorized access to the external service.

## Setup and Running the Application

### Prerequisites
* **.NET 8 SDK:** Specifically version `8.0.411` (as enforced by the `global.json` file in this project's root).
* Node.js and npm/yarn (for the React frontend, if testing full stack).

### Steps
1.  **Clone the repository:**
    ```bash
    git clone <your-repo-url>
    # Navigate to the specific build folder, e.g.:
    cd InputValid-dotnet-secure/dotnet/vulnerable-secrets-management # Example subfolder for this specific build
    ```
2.  **Verify .NET SDK version (optional, but good practice):**
    ```bash
    dotnet --info
    ```
    Ensure it shows `Version: 8.0.411` under ".NET SDKs installed" and "SDK: Version: 8.0.411" for the host. If not, ensure `global.json` is correctly placed in this project's root directory.
3.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```
4.  **Build the application:**
    ```bash
    dotnet build
    ```
5.  **Run the application:**
    ```bash
    dotnet run
    ```
    The application will typically start on `http://localhost:5000`.

## API Endpoints

### `POST /api/generate`
* **Purpose:** Generates a product description by calling an external AI API (OpenAI). In this vulnerable build, it uses a hardcoded API key.
* **Method:** `POST`
* **Content-Type:** `application/json`
* **Request Body Example (JSON):**
    ```json
    {
      "productName": "Smart Watch",
      "keywords": "fitness tracking, long battery life, stylish"
    }
    ```
* **Expected Behavior:**
    * The application will attempt to make an API call to OpenAI using the hardcoded key.
    * If the hardcoded key is valid and has access, it will return a generated description (`200 OK`).
    * If the hardcoded key is invalid (e.g., `FAKE_API_KEY`), it will likely return a `500 Internal Server Error` due to an API authentication failure, potentially exposing details about the attempted API call.

## Static Analysis Tooling
This specific build is designed to be analyzed by Static Analysis Security Testing (SAST) tools such as Semgrep and .NET Roslyn Analyzers to measure their detection capabilities for **hardcoded secrets** vulnerabilities present in this build.
