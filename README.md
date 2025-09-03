# Ketarin

Ketarin is a small application which automatically updates setup packages. As opposed to other tools, Ketarin is not meant to keep your system up-to-date, but rather to maintain a compilation of all important setup packages which can then be burned to disc or put on a USB stick.

I created this application, because I couldn't find anything like it when I needed such a functionality. Since I don't want my efforts go to waste, I decided to release it to the public. Ketarin is open source, so you can also extend its functionality to fit your needs (just note that you may not use the icons that ship with it freely as well). I'd also appreciate source code contributions. Ketarin is written in C#, for .NET 6.0 and uses SQLite as database engine.

## How does it work?

Basically, it monitors the content of web pages for changes and downloads files to a specified location. There is a tutorial explaining it all. Currently, you can either rely on a service based on FileHippo, or you can define your own rules, even using regular expressions (for advanced users). A similar application, for monitoring web pages, is Webmon and has sometimes served as guide.

## Requirements

- Windows 10 or later
- .NET 6.0 Runtime

## Development

[![Build status](https://ci.appveyor.com/api/projects/status/64v9x5oobte4rkaj?svg=true)](https://ci.appveyor.com/project/floele/ketarin)

### Prerequisites

- .NET 6.0 SDK
- Visual Studio 2022 or Visual Studio Code

### Building

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build --configuration Release

# Run the application
dotnet run --project Ketarin.csproj
```

### Recent Updates

- Migrated from .NET Framework 4.5.2 to .NET 6.0
- Updated all dependencies to latest versions
- Improved security with modern HttpClient implementation
- Enhanced SSL/TLS certificate validation
- Updated CI/CD pipelines for .NET 6.0
