# SMPP Client application

A Windows Forms application for sending SMS messages using an SMPP gateway. This application is built with C# and can be compiled using MSBuild.

---

## Compilation

### Prerequisites

1. **Windows OS** with:
   - .NET Framework 4.7 or higher.
   - Visual Studio with MSBuild installed, or standalone MSBuild tools.
2. **Dependencies**:
   - [JamaaSMPP](https://www.nuget.org/packages/JamaaSMPP): Ensure it's installed in your project.
   - [System.Data.SQLite](https://www.nuget.org/packages/System.Data.SQLite/): Ensure it's installed in your project.

### Steps to Compile

1. Clone the repository:

   ```bash
   git clone https://github.com/sorin25/smpp-gui.git
   cd smpp-gui
   ```

2. Restore NuGet packages:

   ```bash
   nuget restore SMPPClient.csproj
   ```

3. Compile using MSBuild:

   ```bash
   msbuild SMPPClient.csproj /p:Configuration=Release
   ```

4. Output:
   - The compiled binaries will be located in the `bin\Release` directory.

---

<!--
## Installation

### Portable Installation

1. Navigate to the `bin\Release` directory.
2. Copy all files in this directory to your desired location.
3. Run `SMPPClient.exe` directly.

### Installer-Based Installation (Optional)

If using an installer:

1. Download the `SMPPClientSetup.exe` file from the [Releases](https://github.com/sorin25/smpp-gui/releases) page.
2. Run the installer and follow the on-screen instructions.
3. Launch the application from the **Start Menu** or the installation directory.

---
-->

## Usage

1. Launch the application.
2. Configure the SMPP settings:
   - Enter `Host`, `Port`, `Username`, and `Password`.
3. Enter the sender and recipient numbers.
4. Type your SMS message.
5. Click **Send SMS** to send the message.

---

## Contribution

1. Fork the repository and create a new branch for your feature or bug fix.
2. Submit a pull request with a detailed description of your changes.

---

## License

This project is licensed under the [**Microsoft Reciprocal License (Ms-RL)**](https://opensource.org/license/ms-rl-html).
You can view the full license text [here](LICENSE).

### JamaaSMPP Dependency

This project uses JamaaSMPP, which is licensed under the Microsoft Reciprocal License (Ms-RL). As such, the entire project is subject to the terms of the Ms-RL.
