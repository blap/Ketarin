# Ketarin Compilation Errors and Warnings Fix Design Document

## 1. Overview

This document outlines the approach to fix all compilation errors and warnings in the Ketarin project while minimizing dependencies and ensuring portability. The project is being migrated from .NET Framework 4.5.2 to .NET 9.0, which introduces several compatibility issues that need to be addressed. Special attention will be paid to reducing external dependencies and ensuring the application remains portable. SQLite has been removed and replaced with JSON-based storage for maximum portability.

## 2. Architecture

The solution involves addressing issues in the following areas while minimizing dependencies:
1. Missing package references and dependencies
2. Namespace and type resolution issues
3. Obsolete API usage
4. Ambiguous type references
5. Type conversion issues
6. Missing method implementations

## 2.1 Dependency Reduction Strategy

To minimize dependencies and ensure portability:
1. Use built-in .NET functionality wherever possible
2. Replace third-party libraries with standard alternatives
3. Implement custom solutions for simple functionality
4. Eliminate SQLite dependency by using JSON-based storage
5. Ensure all dependencies are compatible with .NET 9.0

## 3. Error Categories and Solutions

### 3.1 Database Layer Issues
**Problem**: Multiple errors related to missing SQLite types like `SQLiteConnection`, `SQLiteTransaction`, `SQLiteParameter` after removing SQLite dependency
**Root Cause**: SQLite has been removed and replaced with JSON-based storage
**Solution**:
- Replace all SQLite-related code with JSON serialization
- Update DbManager to use System.Text.Json for data persistence
- Implement custom database operations using file I/O and JSON
- Address nullability warnings in JSON serialization code

### 3.2 Ambiguous Type References
**Problem**: Errors like `CS0104: "ContentAlignment" is an ambiguous reference between "System.Drawing.ContentAlignment" and "System.Windows.Forms.VisualStyles.ContentAlignment"`
**Root Cause**: Multiple namespaces containing types with the same name
**Solution**:
- Use fully qualified type names
- Add specific using aliases to resolve conflicts

### 3.3 ScintillaNET Issues
**Problem**: Missing `LexerLanguage` and other Scintilla properties
**Root Cause**: ScintillaNET has been replaced with built-in TextBox controls
**Solution**:
- Replace ScintillaNET with RichTextBox for basic text editing
- Implement custom syntax highlighting if needed
- Remove Scintilla-specific properties and methods

### 3.4 XML-RPC Issues
**Problem**: Missing `IKetarinRpc` and `XmlRpcProxyGen` types
**Root Cause**: XML-RPC functionality has been disabled for .NET 9 compatibility
**Solution**:
- Remove XML-RPC.NET NuGet package
- Permanently disable online database functionality
- Remove all XML-RPC related code

### 3.5 SharpSSH/SCP Issues
**Problem**: Tamir.SharpSsh.dll dependency is not compatible with .NET 9
**Root Cause**: SharpSSH library has been removed
**Solution**:
- Remove SharpSSH dependency
- Disable SCP protocol support
- Update ScpWebRequest to return appropriate error messages

### 3.3 Missing Types and Namespaces
**Problem**: Errors like `CS0246: The type or namespace name "MenuItem" could not be found`
**Root Cause**: API changes between .NET Framework and .NET
**Solution**:
- Replace deprecated types with their .NET equivalents (ToolStripItem instead of MenuItem)
- Add missing using statements
- Update code to use new APIs

### 3.4 Obsolete API Usage
**Problem**: Warnings like `SYSLIB0011: BinaryFormatter is obsolete`
**Root Cause**: Security concerns with BinaryFormatter in modern .NET
**Solution**:
- Replace BinaryFormatter with safer alternatives like JSON serialization
- Use System.Text.Json for serialization needs (built into .NET 9.0)

### 3.5 XML-RPC Related Issues
**Problem**: Missing `IKetarinRpc` and `XmlRpcProxyGen` types
**Root Cause**: XML-RPC functionality has been disabled for .NET 9 compatibility
**Solution**:
- Replace XML-RPC functionality with XML-RPC.NET NuGet package
- Re-enable online database functionality
- Update references to use proper namespace

### 3.6 ScintillaNET Issues
**Problem**: Missing `LexerLanguage` and other Scintilla properties
**Root Cause**: ScintillaNET has been replaced with built-in TextBox controls
**Solution**:
- Replace ScintillaNET with built-in RichTextBox for basic syntax highlighting
- Implement custom syntax highlighting if needed
- Adjust code to match new API signatures

### 3.7 Missing System Types
**Problem**: Errors like `CS0246: The type or namespace name "StringBuilder" could not be found`
**Root Cause**: Missing using statements for System.Text
**Solution**:
- Add `using System.Text;` to affected files

### 3.8 Property Access Issues
**Problem**: Errors like `CS0117: "DbManager" does not contain a definition for "Connection"`
**Root Cause**: Refactoring of database manager properties and methods after removing SQLite
**Solution**:
- Update property access to use correct method names
- Fix method signatures to match new JSON-based API

### 3.9 Type Conversion Issues
**Problem**: Errors like `CS0029: Cannot implicitly convert type "string" to "Ketarin.ScriptType"`
**Root Cause**: Stricter type checking in .NET 9.0
**Solution**:
- Add explicit type casting
- Update property setters to handle type conversion properly

### 3.10 PowerShell Dependencies
**Problem**: PowerShell reference assemblies may not be available on all systems
**Root Cause**: PowerShell is not always installed on target systems
**Solution**:
- Make PowerShell functionality optional
- Check for PowerShell availability before use
- Provide alternative scripting mechanisms

## 4. Detailed Fix Implementation Plan

### 4.1 Update Project File Dependencies
```xml
<ItemGroup>
  <!-- SQLite removed - using JSON for storage -->
  <!-- ScintillaNET removed - using RichTextBox -->
  <!-- <PackageReference Include="jacobslusser.ScintillaNET" Version="3.6.3" /> -->
  <!-- SharpSSH removed - SCP protocol disabled -->
  <!-- XML-RPC.NET removed - online database functionality disabled -->
  <!-- Optional: Only if PowerShell scripting is required -->
  <!-- <PackageReference Include="Microsoft.PowerShell.5.ReferenceAssemblies" Version="1.1.0" /> -->
</ItemGroup>
```

### 4.2 Implement JSON-Based Database Layer
1. Remove all SQLite-related using statements
2. Add JSON serialization using System.Text.Json:
   ```csharp
   using System.Text.Json;
   ```
3. Update DbManager to use file I/O and JSON serialization instead of SQLite
4. Implement custom database operations for CRUD functionality
5. Replace all SQLiteConnection, SQLiteTransaction, and SQLiteParameter usage with JSON equivalents
6. Address nullability warnings in JSON serialization code

### 4.3 Resolve Ambiguous References
1. For ContentAlignment issues in SplitButton.cs:
   ```csharp
   using ContentAlignment = System.Drawing.ContentAlignment;
   ```
2. For MethodInvoker issues in ObjectListView.cs:
   ```csharp
   using MethodInvoker = System.Windows.Forms.MethodInvoker;
   ```

### 4.4 Replace Obsolete BinaryFormatter Usage
1. In CDBurnerXP/Settings.cs and CDBurnerXP/ObjectListView.cs:
   - Replace BinaryFormatter with System.Text.Json serialization
   - Implement custom serialization methods

### 4.5 Permanently Disable XML-RPC Functionality
1. Remove XML-RPC.NET package reference
2. Remove all using statements related to CookComputing.XmlRpc
3. Permanently disable online database functionality in ApplicationJobDialog.cs, ImportFromDatabaseDialog.cs, and Updater.cs
4. Remove all references to XmlRpcProxyGen and related types

### 4.6 Replace ScintillaNET with RichTextBox
1. Replace TextBox controls with RichTextBox for syntax highlighting
2. Implement custom syntax highlighting for different script types
3. Update CommandControl.cs to use RichTextBox API
4. Remove Scintilla-specific properties like `LexerLanguage`

### 4.7 Remove SharpSSH Dependency
1. Remove all references to SharpSSH in the codebase
2. Update ScpWebRequest to properly handle disabled protocol
3. Ensure no compilation errors related to missing SharpSSH types

### 4.7 Fix Missing Types
1. Replace MenuItem with ToolStripMenuItem
2. Add using statements for missing types like StringBuilder
3. Fix property access issues in ApplicationJob and DbManager

### 4.8 Make PowerShell Optional
1. Check for PowerShell availability before use
2. Provide fallback mechanisms for scripting
3. Handle cases where PowerShell is not available gracefully

### 4.9 Specific File Fixes

#### SplitButton.cs
- Add `using ContentAlignment = System.Drawing.ContentAlignment;` to resolve ambiguous references

#### ObjectListView.cs
- Add `using MethodInvoker = System.Windows.Forms.MethodInvoker;` to resolve ambiguous references
- Replace BinaryFormatter serialization with JSON serialization

#### CommandControl.cs
- Add `using System.Text;` to resolve StringBuilder issues
- Replace ScintillaNET with RichTextBox controls
- Remove Scintilla-specific properties like `LexerLanguage`
- Implement custom syntax highlighting

#### ApplicationJob.cs
- Replace all SQLite related method calls with JSON equivalents
- Correct property access for DateAdded, PreviousRelativeLocation, etc.
- Update data persistence methods to use JSON serialization

#### DbManager.cs
- Replace SQLite implementation with JSON-based storage
- Remove Connection, FormatGuid, and other SQLite-specific properties/methods
- Implement JSON serialization for all database operations
- Correct method signatures and return types for JSON-based operations

#### JsonDbManager.cs
- Verify JSON-based storage implementation is complete
- Address nullability warnings in JSON serialization code
- Implement proper error handling for file I/O operations

#### Scp.cs
- Verify SCP protocol is properly disabled
- Ensure no references to SharpSSH library
- Return appropriate error messages for SCP URIs

#### Forms/*.cs
- Replace MenuItem with ToolStripMenuItem
- Fix XML-RPC related issues or remove if not essential

#### UrlVariable.cs
- Fix JsonDbManager method calls
- Address nullability warnings

#### PowerShellScript.cs
- Add PowerShell availability checks
- Implement fallback scripting mechanisms

## 5. Business Logic Layer

### 5.1 Database Layer Fixes
- Update all database access methods to use proper SQLite types
- Fix transaction handling
- Correct parameter binding in SQL commands

### 5.2 UI Layer Fixes
- Resolve all ambiguous control references
- Update deprecated UI controls
- Fix property access for custom controls

### 5.3 Serialization Layer Fixes
- Replace all BinaryFormatter usage with JSON serialization
- Update settings persistence mechanisms
- Ensure backward compatibility where possible

## 6. Testing Strategy

### 6.1 Unit Testing
- Verify SQLite database operations work correctly
- Test serialization/deserialization functionality
- Validate UI control interactions

### 6.2 Integration Testing
- Test complete application job lifecycle
- Verify download functionality with all protocol providers
- Check setup instruction execution

### 6.3 Regression Testing
- Ensure all existing functionality remains intact
- Verify configuration import/export works correctly
- Test command execution and scripting features

### 6.4 Build Verification
- Verify all compilation errors are resolved
- Confirm no new warnings are introduced
- Test application startup and basic functionality

### 6.5 Portability Testing
- Test on clean Windows installation without optional dependencies
- Verify application works without PowerShell installed
- Confirm JSON-based storage functionality without additional drivers
- Test on different Windows versions (Windows 10, Windows 11)
- Verify single executable deployment works correctly

## 7. Migration Checklist

### 7.1 Package Dependencies
- [x] Remove System.Data.SQLite.Core NuGet package (replaced with JSON storage)
- [x] Remove XML-RPC.NET NuGet package (online database functionality disabled)
- [x] Remove ScintillaNET dependency (replaced with RichTextBox)
- [ ] Evaluate PowerShell reference assemblies requirement
- [x] Remove Tamir.SharpSsh.dll dependency (SCP protocol disabled)
- [ ] Remove all non-essential dependencies

### 7.2 Code Fixes
- [x] Replace all SQLite related code with JSON-based implementation
- [ ] Fix ambiguous type references
- [ ] Replace obsolete BinaryFormatter usage
- [x] Disable XML-RPC API functionality
- [x] Replace ScintillaNET with RichTextBox controls
- [ ] Address missing type issues
- [ ] Correct property access problems
- [ ] Make PowerShell functionality optional
- [ ] Implement JSON-based data persistence for all entities
- [ ] Address nullability warnings in JSON serialization code
- [x] Remove SharpSSH/SCP protocol implementation

### 7.3 TODO for Qoder
- [ ] Review and finalize JSON-based database implementation
- [ ] Implement custom syntax highlighting for RichTextBox
- [ ] Address all nullability warnings in the codebase
- [ ] Verify all compilation errors are resolved
- [ ] Conduct final testing of all core functionality

### 7.3 Testing
- [ ] Verify successful build with no errors or warnings
- [ ] Run unit tests
- [ ] Execute integration tests
- [ ] Perform regression testing
- [ ] Validate application functionality
- [ ] Test on clean system without optional dependencies
- [ ] Verify portability across different Windows versions
- [ ] Test JSON-based storage functionality
- [ ] Verify data persistence and retrieval works correctly
- [ ] Test RichTextBox syntax highlighting