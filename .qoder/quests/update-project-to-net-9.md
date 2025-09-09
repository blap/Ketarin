# Update Ketarin Project to .NET 9

## Overview

This document outlines the plan to migrate the Ketarin application from .NET Framework 4.5.2 to .NET 9. Ketarin is a Windows Forms application that automatically updates setup packages. The migration will modernize the codebase, improve performance, and ensure long-term maintainability.

## Architecture

### Current Technology Stack
- **Language**: C#
- **Framework**: .NET Framework 4.5.2
- **UI Framework**: Windows Forms
- **Database**: SQLite
- **Key Dependencies**:
  - ScintillaNET for text editing
  - System.Data.SQLite.Core for database access
  - Microsoft.PowerShell for script execution
  - Tamir.SharpSsh for SSH functionality

### Dependency Reduction Goal
- Minimize external dependencies to the greatest extent possible
- Target zero external dependencies where feasible
- Ensure the application remains fully portable
- Embed required libraries directly when possible

### Project Structure
The application follows a modular architecture with the following key components:
- **CDBurnerXP**: Custom UI controls and utility components
- **Downloader**: Core downloading logic with protocol support (HTTP, FTP)
- **Forms**: Windows Forms UI components
- **XmlRpc**: XML-RPC implementation for remote communication

## Migration Strategy

### 1. Target Framework Migration
The project will be migrated from .NET Framework 4.5.2 to .NET 9, which requires:
- Converting the project file format from the legacy .csproj format to the SDK-style format
- Updating all NuGet package references to .NET 9 compatible versions
- Addressing API differences between .NET Framework and .NET

### 2. Dependencies Assessment
| Current Package | .NET 9 Compatible Version | Migration Approach | Dependency Priority |
|-----------------|---------------------------|-------------------|------------------|
| ScintillaNET | ScintillaNET or alternative | Evaluate newer versions | Low - Consider removal |
| System.Data.SQLite.Core | System.Data.SQLite.Core 1.0.118+ | Direct upgrade | Medium - Consider embedded |
| Microsoft.PowerShell.5.ReferenceAssemblies | Microsoft.PowerShell.SDK 7.x | Complete replacement | Low - Consider alternatives |
| Tamir.SharpSsh.dll | SSH.NET | Replacement with modern alternative | Low - Consider removal |

### Dependency Minimization Strategy
- Remove non-essential dependencies entirely
- Embed critical dependencies directly into the application
- Use built-in .NET 9 functionality where possible
- Implement custom solutions for specialized features
- Ensure all remaining dependencies are portable
- Target zero external dependencies for maximum portability

### 3. Code Modernization
Key areas requiring updates:
- Windows Forms compatibility with .NET 9
- Threading model updates (Dispatcher to async/await)
- File I/O operations modernization
- String and collection API updates
- Configuration system migration (app.config to appsettings.json)
- Dependency removal and replacement with built-in functionality

## Detailed Migration Steps

### Phase 1: Project Structure and SDK Migration
1. Convert Ketarin.csproj to SDK-style format
2. Update target framework to net9.0-windows
3. Enable Windows Forms support with appropriate MSBuild properties
4. Migrate assembly attributes from AssemblyInfo.cs to project file
5. Update solution file to newer Visual Studio format

### Phase 2: Dependency Updates
1. Replace packages.config with PackageReference format
2. Update System.Data.SQLite.Core to .NET 9 compatible version or embed SQLite
3. Replace Microsoft.PowerShell.5.ReferenceAssemblies with Microsoft.PowerShell.SDK or implement custom scripting
4. Replace Tamir.SharpSsh with SSH.NET or implement custom SSH functionality
5. Update or replace ScintillaNET with .NET 9 compatible version or remove text editing feature
6. Evaluate and remove non-essential dependencies
7. Embed critical dependencies directly into the application

### Phase 3: Code Modernization
1. Update WinForms designer code for .NET 9 compatibility
2. Replace synchronous patterns with async/await where applicable
3. Modernize threading code to use Task-based patterns
4. Update file I/O operations to use async methods
5. Replace obsolete APIs with modern equivalents

### Phase 4: Configuration Migration
1. Convert app.config to appsettings.json
2. Update configuration access code
3. Migrate startup settings and runtime configuration

### Phase 5: Testing and Validation
1. Unit testing with .NET 9 runtime
2. UI testing of all forms and controls
3. Functional testing of download and update features
4. Performance benchmarking

## API Changes and Compatibility

### Breaking Changes
- Some .NET Framework specific APIs may not be available in .NET 9
- Windows Forms behavior changes between .NET Framework and .NET
- Registry access and file system APIs may require updates
- COM interop and P/Invoke signatures may need adjustment

### Windows Forms Considerations
- High DPI support improvements in .NET 9
- Updated default font and scaling behavior
- Enhanced accessibility features
- Improved theming support

## Data Models & Database

The application uses SQLite for local data persistence. The migration requires:
- Updating System.Data.SQLite to .NET 9 compatible version
- Validating database schema compatibility
- Testing data access layer functionality
- Ensuring connection string compatibility

## Business Logic Layer

### Core Components
1. **ApplicationJob**: Main entity representing applications to track
2. **Downloader**: Core download engine with protocol support
3. **Updater**: Update checking and management logic
4. **DbManager**: Database access layer

### Migration Considerations
- Preserve existing business logic functionality
- Modernize async patterns in download operations
- Maintain backward compatibility with existing database
- Replace PowerShell script execution with built-in .NET functionality
- Remove SSH functionality or implement custom solution

## UI Architecture

### Component Hierarchy
- **MainForm**: Primary application window
- **Dialog Forms**: Various configuration and management dialogs
- **Custom Controls**: CDBurnerXP library components
- **User Controls**: Specialized UI components

### Migration Tasks
- Update all forms to .NET 9 Windows Forms
- Modernize event handling patterns
- Update control initialization code
- Validate custom control compatibility

## Middleware & Services

### Protocol Providers
- HTTP/HTTPS protocol support
- FTP protocol support
- Custom Ketarin protocol
- FileHippo integration

### Migration Requirements
- Update web request handling for .NET 9
- Validate SSL/TLS protocol compatibility
- Modernize authentication mechanisms
- Update proxy configuration handling
- Replace external SSH library with custom implementation or remove feature

## Testing Strategy

### Unit Testing
- Migrate existing tests to .NET 9
- Add tests for new async patterns
- Validate database operations
- Test configuration migration

### Integration Testing
- End-to-end download workflows
- UI interaction testing
- PowerShell script execution
- Database migration scenarios

### Performance Testing
- Download performance benchmarks
- Memory usage analysis
- Startup time measurements
- UI responsiveness validation

## Deployment Considerations

### Installation
- Single-file deployment options
- Native AOT compilation possibilities
- Self-contained deployment to eliminate runtime dependencies
- Fully portable application distribution

### Runtime Requirements
- Self-contained deployment eliminates runtime dependencies
- Windows compatibility (Windows 10 1607+ or Windows 11)
- No external PowerShell dependency required

## Rollback Plan

If critical issues are discovered:
1. Maintain backup of .NET Framework 4.5.2 version
2. Implement feature flagging for gradual rollout
3. Prepare rollback procedure to previous version
4. Document compatibility issues and workarounds

## Timeline and Milestones

### Phase 1: Project Setup (1 week)
- SDK-style project conversion
- Dependency assessment and planning

### Phase 2: Core Migration (2 weeks)
- Dependency updates
- Basic functionality restoration

### Phase 3: Code Modernization (2 weeks)
- Async/await implementation
- API modernization

### Phase 4: Testing and Validation (1 week)
- Comprehensive testing
- Bug fixes and optimization

### Phase 5: Deployment Preparation (1 week)
- Documentation updates
- Deployment package creation

## Risks and Mitigation

### Technical Risks
1. **Dependency Compatibility**: Some libraries may not have .NET 9 versions
   - Mitigation: Identify alternatives early, implement wrappers if needed

2. **Windows Forms Breaking Changes**: UI behavior differences
   - Mitigation: Thorough testing of all forms and controls

3. **PowerShell Integration**: Changes in PowerShell hosting
   - Mitigation: Replace with built-in .NET functionality

4. **Dependency Removal**: Challenges in replacing critical dependencies
   - Mitigation: Implement custom solutions or simplify functionality

### Schedule Risks
1. **Extended Testing Phase**: Unexpected compatibility issues
   - Mitigation: Buffer time in schedule, prioritize critical features

2. **Dependency Delays**: Waiting for library updates
   - Mitigation: Use alternative libraries or implement required functionality directly

3. **Dependency Removal Complexity**: Time required to replace or remove dependencies
   - Mitigation: Plan additional time for custom implementation, consider feature removal

## Success Criteria

1. Application compiles and runs on .NET 9
2. All existing functionality preserved
3. Performance improvements achieved
4. Successful automated build and deployment
5. Zero external dependencies achieved
6. Fully portable application
7. Passing test suite with adequate coverage
8. Positive user acceptance testing results

## TODO

- [ ] Convert Ketarin.csproj to SDK-style format
- [ ] Update target framework to net9.0-windows
- [ ] Replace packages.config with PackageReference format
- [ ] Evaluate System.Data.SQLite.Core replacement options
- [ ] Implement custom scripting solution to replace PowerShell
- [ ] Implement custom SSH functionality or remove feature
- [ ] Evaluate ScintillaNET replacement or removal
- [ ] Update WinForms designer code for .NET 9 compatibility
- [ ] Replace synchronous patterns with async/await
- [ ] Convert app.config to appsettings.json
- [ ] Test self-contained deployment
- [ ] Validate zero external dependencies
- [ ] Conduct performance benchmarking
- [ ] Execute comprehensive testing