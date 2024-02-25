
# How to Install Docker Desktop for Windows and Set Up WSL 2

## Prerequisites
- Windows 10 64-bit: Home, Pro, Enterprise, or Education, build 18362 or later.
- Enable the WSL 2 feature on Windows. For this, you need to join the Windows Insider Program.

## Step 1: Enable Windows Subsystem for Linux (WSL)
1. Open PowerShell as Administrator.
2. Run the following command to enable WSL:
   ```powershell
   dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
   ```

## Step 2: Update to WSL 2
1. Ensure that you are using Windows 10, version 2004 and higher (Build 18362 or higher) for x64 systems.
2. Run PowerShell as Administrator and execute:
   ```powershell
   dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
   ```
3. Restart your machine.
4. Download and install the [Linux kernel update package](https://aka.ms/wsl2kernel).

## Step 3: Set WSL 2 as Your Default Version
1. Open PowerShell as Administrator.
2. Run this command:
   ```powershell
   wsl --set-default-version 2
   ```

## Step 4: Install a Linux Distribution
1. Open Microsoft Store and select your favorite Linux distribution (e.g., Ubuntu, Debian, etc.).
2. Click "Get" to download and install the distribution.

## Step 5: Install Docker Desktop for Windows
1. Download Docker Desktop for Windows from the [Docker Hub](https://hub.docker.com/editions/community/docker-ce-desktop-windows/).
2. Double-click the installer and follow the on-screen instructions.
3. Ensure that the 'WSL 2 Tech Preview' option is selected during installation.

## Step 6: Configure Docker to Use WSL 2
1. After installation, Docker will ask to use WSL 2. Confirm this.
2. You can also right-click on the Docker taskbar item, select 'Settings', go to the 'General' tab, and ensure 'Use the WSL 2 based engine' is checked.

## Step 7: Verify the Installation
1. Open Docker Desktop to ensure it starts correctly.
2. Open a terminal in your WSL 2 Linux distribution.
3. Try running a test Docker command, e.g., `docker run hello-world`.

## Conclusion
You now have Docker Desktop installed with WSL 2 on your Windows machine. You can start using Docker containers within your Linux distributions on Windows. Remember, Docker with WSL 2 provides better performance and integrates seamlessly with your Windows environment.
