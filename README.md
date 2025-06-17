AdScribe.AI - .NET 8/C# (Vulnerable Version)
This repository contains the .NET 8/C# stack version of the AdScribe.AI application. This version is intentionally vulnerable and serves as a test case for a university research project evaluating the effectiveness of secure coding practices and SAST (Static Analysis Security Testing) tools.

Application Purpose
AdScribe.AI is a simple marketing tool that uses the OpenAI API to generate compelling product descriptions based on a product name and user-provided keywords.

Research Context: The Vulnerability
The primary purpose of this repository is to demonstrate an unsecure but common coding practice: hardcoded secrets.

In this application, the OPENAI_API_KEY is written directly into the backend source code (backend/Controllers/GenerateController.cs). This is a significant security risk because it exposes the secret to anyone with access to the codebase and makes it visible in the version control history. This build is used to test whether security scanning tools can successfully detect this type of vulnerability.

How to Run This Application
This is a standard full-stack application with a React frontend and a .NET 8 backend.

Prerequisites
.NET 8 SDK installed.

Node.js and npm installed.

An active OpenAI API key.

Instructions
1. Clone the Repository
git clone <your-repo-url>

2. Set the API Key (The Unsecure Step)
Navigate to the backend/ directory.

Open the Controllers/GenerateController.cs file.

Find the line const string OPENAI_API_KEY = "REPLACE_WITH_YOUR_OPENAI_API_KEY";

Replace the placeholder with your actual OpenAI API key.

3. Set up the Frontend (React)
In a terminal, navigate to the frontend/ folder.

Install the Node.js dependencies:

npm install

4. Run the Application
You will need two separate terminals to run the application.

Start the Backend:

In a terminal, navigate to the backend/ directory.

Run the .NET server:

dotnet run

The backend will start on http://localhost:5001.

Start the Frontend:

In your second terminal, navigate to the frontend/ directory.

Run the React development server:

npm start

The application will open in your browser, usually at http://localhost:3000.
