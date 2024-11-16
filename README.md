# Simple Dotnet Project

```
dotnet new sln -n SimpleDotNet9Solution

dotnet new webapi -n SimpleDotNet9WebApi

dotnet sln add ./SimpleDotNet9WebApi/SimpleDotNet9WebApi.csproj

<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <LangVersion>13.0</LangVersion>
  </PropertyGroup>

</Project>

dotnet build

dotnet run --project SimpleDotNet9WebApi
```
Yes, you can have multiple .NET versions installed on your system and switch between them easily. .NET SDKs are designed to coexist, allowing you to target different versions for different projects.

To switch between .NET versions, you can use the `global.json` file to specify the SDK version for a particular project. Here’s how you can do it:

1. **Create a `global.json` file** in the root of your project directory:

```sh
dotnet new globaljson --sdk-version <version>
```

Replace `<version>` with the desired .NET SDK version (e.g., `9.0.100`).

2. **Edit the `global.json` file** to specify the SDK version:

```json
{
  "sdk": {
    "version": "9.0.100"
  }
}
```

This ensures that the specified .NET SDK version is used when you build and run the project.

3. **Verify the SDK version** being used by running the following command in the project directory:

```
dotnet --version
8.0.302

dotnet --version
9.0.100
```

This will display the SDK version specified in the `global.json` file.

By using the `global.json` file, you can easily switch between different .NET versions for different projects.


```
➜ dotnet build    

Welcome to .NET 9.0!
---------------------
SDK Version: 9.0.100

Telemetry
---------
The .NET tools collect usage data in order to help us improve your experience. It is collected by Microsoft and shared with the community. You can opt-out of telemetry by setting the DOTNET_CLI_TELEMETRY_OPTOUT environment variable to '1' or 'true' using your favorite shell.

Read more about .NET CLI Tools telemetry: https://aka.ms/dotnet-cli-telemetry

----------------
Installed an ASP.NET Core HTTPS development certificate.
To trust the certificate, run 'dotnet dev-certs https --trust'
Learn about HTTPS: https://aka.ms/dotnet-https

----------------
Write your first app: https://aka.ms/dotnet-hello-world
Find out what's new: https://aka.ms/dotnet-whats-new
Explore documentation: https://aka.ms/dotnet-docs
Report issues and find source on GitHub: https://github.com/dotnet/core
Use 'dotnet --help' to see available commands or visit: https://aka.ms/dotnet-cli
--------------------------------------------------------------------------------------
An issue was encountered verifying workloads. For more information, run "dotnet workload update".
Restore complete (3,3s)
  simple-dotnet-service succeeded (2,6s) → bin/Debug/net9.0/simple-dotnet-service.dll

Build succeeded in 7,1s
```

```
➜ dotnet run              
Using launch settings from /Users/ias/RiderProjects/simple-dotnet-service/simple-dotnet-service/Properties/launchSettings.json...
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5048
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /Users/ias/RiderProjects/simple-dotnet-service/simple-dotnet-service
```