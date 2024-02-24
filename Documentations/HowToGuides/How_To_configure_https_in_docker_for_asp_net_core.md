
# Configuring HTTPS in Docker for ASP.NET Core Applications

This guide outlines the steps to set up HTTPS within a Docker container for an ASP.NET Core application, using SSL certificates.

## Step 1: Obtain an SSL Certificate
Obtain an SSL certificate from a trusted Certificate Authority (CA) for production use. For development or testing, you can create a self-signed certificate.

### Creating a Self-Signed Certificate (for Development/Test)
Use the following commands to create a self-signed certificate:

```bash
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p [your_password]
dotnet dev-certs https --trust
```

## Step 2: Copy the Certificate into the Docker Image
Modify your Dockerfile to include the certificate.

```Dockerfile
# Copy the certificate
COPY ["path/to/your/aspnetapp.pfx", "/https/"]
```

## Step 3: Configure ASP.NET Core to Use the Certificate
Configure Kestrel in `Program.cs` to use the SSL certificate for HTTPS.

```csharp
var certPassword = Environment.GetEnvironmentVariable("CERT_PASSWORD");

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 443, listenOptions =>
    {
        listenOptions.UseHttps("/https/aspnetapp.pfx", certPassword);
    });
});
```

## Step 4: Set the Certificate Password in Docker-Compose
In your `docker-compose.yml`, add the certificate password as an environment variable.

```yaml
services:
  vivageoapi:
    environment:
      - CERT_PASSWORD=[your_password]
```

## Step 5: Build and Run Your Docker Container
Build and run your Docker container to serve HTTPS content.

## Security Notes:
- Handle SSL certificates securely, especially in production.
- Self-signed certificates are not trusted by default and may require additional steps to be trusted on the client-side.
- Ensure correct port mapping for HTTPS in `docker-compose.yml`.
