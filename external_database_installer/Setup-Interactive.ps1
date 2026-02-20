<#
.SYNOPSIS
    ATSPM 5.0 External Database Setup - Interactive Wizard

.DESCRIPTION
    User-friendly step-by-step wizard to configure and set up external databases for ATSPM 5.0
#>

$ErrorActionPreference = "Stop"

# Colors
function Write-Title {
    param([string]$Message)
    Write-Host ""
    Write-Host "==========================================================================" -ForegroundColor Cyan
    Write-Host " $Message" -ForegroundColor Cyan
    Write-Host "==========================================================================" -ForegroundColor Cyan
    Write-Host ""
}

function Write-Step {
    param([string]$Message)
    Write-Host ""
    Write-Host "> $Message" -ForegroundColor Yellow
}

function Write-Info {
    param([string]$Message)
    Write-Host "  $Message" -ForegroundColor Gray
}

function Write-Success {
    param([string]$Message)
    Write-Host "[OK] $Message" -ForegroundColor Green
}

function Write-Error {
    param([string]$Message)
    Write-Host "[ERROR] $Message" -ForegroundColor Red
}

function Read-Choice {
    param(
        [string]$Prompt,
        [string[]]$Options,
        [int]$Default = 1
    )
    
    Write-Host ""
    Write-Host $Prompt -ForegroundColor Yellow
    for ($i = 0; $i -lt $Options.Length; $i++) {
        if (($i + 1) -eq $Default) {
            Write-Host "  $($i + 1). $($Options[$i]) [Default]" -ForegroundColor Green
        } else {
            Write-Host "  $($i + 1). $($Options[$i])"
        }
    }
    
    do {
        Write-Host ""
        $choice = Read-Host "Enter choice (1-$($Options.Length)) or press Enter for default"
        
        if ([string]::IsNullOrWhiteSpace($choice)) {
            return $Default
        }
        
        if ($choice -match '^\d+$' -and [int]$choice -ge 1 -and [int]$choice -le $Options.Length) {
            return [int]$choice
        }
        
        Write-Host "Invalid choice. Please enter a number between 1 and $($Options.Length)" -ForegroundColor Red
    } while ($true)
}

function Read-Value {
    param(
        [string]$Prompt,
        [string]$Default = "",
        [switch]$Required,
        [switch]$Secure
    )
    
    Write-Host ""
    if ([string]::IsNullOrWhiteSpace($Default)) {
        if ($Required) {
            Write-Host "${Prompt} (required): " -ForegroundColor Yellow -NoNewline
        } else {
            Write-Host "${Prompt}: " -ForegroundColor Yellow -NoNewline
        }
    } else {
        Write-Host "${Prompt} [Default: $Default]: " -ForegroundColor Yellow -NoNewline
    }
    
    do {
        if ($Secure) {
            $value = Read-Host -AsSecureString
            $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($value)
            $plainText = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)
            [System.Runtime.InteropServices.Marshal]::ZeroFreeBSTR($BSTR)
            
            if ([string]::IsNullOrWhiteSpace($plainText) -and -not [string]::IsNullOrWhiteSpace($Default)) {
                return $Default
            }
            
            if ([string]::IsNullOrWhiteSpace($plainText) -and $Required) {
                Write-Host "This field is required. Please enter a value: " -ForegroundColor Red -NoNewline
                continue
            }
            
            return $plainText
        } else {
            $value = Read-Host
            
            if ([string]::IsNullOrWhiteSpace($value) -and -not [string]::IsNullOrWhiteSpace($Default)) {
                return $Default
            }
            
            if ([string]::IsNullOrWhiteSpace($value) -and $Required) {
                Write-Host "This field is required. Please enter a value: " -ForegroundColor Red -NoNewline
                continue
            }
            
            return $value
        }
    } while ($true)
}

function Test-PasswordStrength {
    param([string]$Password)
    
    $errors = @()
    
    # Minimum length
    if ($Password.Length -lt 6) {
        $errors += "Password must be at least 6 characters"
    }
    
    # Must have lowercase
    if ($Password -cnotmatch '[a-z]') {
        $errors += "Password must have at least one lowercase letter (a-z)"
    }
    
    # Must have uppercase
    if ($Password -cnotmatch '[A-Z]') {
        $errors += "Password must have at least one uppercase letter (A-Z)"
    }
    
    # Must have digit
    if ($Password -notmatch '\d') {
        $errors += "Password must have at least one digit (0-9)"
    }
    
    # Must have non-alphanumeric
    if ($Password -notmatch '[^a-zA-Z0-9]') {
        $errors += "Password must have at least one special character (!@#$%^&*)"
    }
    
    if ($errors.Count -gt 0) {
        return @{
            Valid = $false
            Errors = $errors
        }
    }
    
    return @{
        Valid = $true
        Errors = @()
    }
}

function Confirm-Action {
    param([string]$Message)
    
    Write-Host ""
    Write-Host "$Message (Y/N): " -ForegroundColor Yellow -NoNewline
    
    do {
        $response = Read-Host
        
        if ($response -match '^[Yy]') {
            return $true
        } elseif ($response -match '^[Nn]') {
            return $false
        }
        
        Write-Host "Please enter Y or N: " -ForegroundColor Red -NoNewline
    } while ($true)
}

# Main wizard
Clear-Host

Write-Title "ATSPM 5.0 External Database Setup Wizard"

Write-Host "Welcome! This wizard will guide you through setting up ATSPM databases" -ForegroundColor Cyan
Write-Host "on your external SQL Server or PostgreSQL server." -ForegroundColor Cyan
Write-Host ""
Write-Host "Developed by: ACTIONLAB at UT Arlington" -ForegroundColor Gray
Write-Host "Author: sladewang" -ForegroundColor Gray
Write-Host ""
Write-Host "What will be created:" -ForegroundColor White
Write-Info '  - 4 ATSPM databases (Config, Aggregation, EventLogs, Identity)'
Write-Info '  - All database tables and schemas'
Write-Info '  - Reference data (products, detection types, measure types, etc.)'
Write-Info '  - Identity roles and permissions'
Write-Info '  - Admin user account'
Write-Info '  - Configuration file for Docker Compose'
Write-Host ""

if (-not (Confirm-Action "Ready to begin?")) {
    Write-Host ""
    Write-Host "Setup cancelled." -ForegroundColor Yellow
    Write-Host ""
    pause
    exit 0
}

# Step 1: Database Type
Write-Title "Step 1 of 7: Select Database Type"

$dbChoice = Read-Choice -Prompt "Which database server are you using?" -Options @("SQL Server", "PostgreSQL") -Default 1

if ($dbChoice -eq 1) {
    $DatabaseType = "SqlServer"
    $defaultPort = 1433
    $defaultUsername = "sa"
} else {
    $DatabaseType = "PostgreSQL"
    $defaultPort = 5432
    $defaultUsername = "postgres"
}

Write-Success "Selected: $DatabaseType"

# Step 2: Connection Method (SQL Server only)
if ($DatabaseType -eq "SqlServer") {
    Write-Title "Step 2 of 7: SQL Server Connection Method"
    
    Write-Info "Choose how to connect to your SQL Server:"
    Write-Info "  - IP/Hostname + Port: For default instances or when using TCP/IP with specific port"
    Write-Info "  - Named Instance: For named SQL Server instances (e.g., SERVER\\INSTANCENAME)"
    
    $connectionChoice = Read-Choice -Prompt "Select connection method" -Options @("IP/Hostname + Port", "Named Instance") -Default 1
    
    if ($connectionChoice -eq 1) {
        $UseNamedInstance = $false
        Write-Success "Selected: IP/Hostname + Port"
    } else {
        $UseNamedInstance = $true
        Write-Success "Selected: Named Instance"
    }
} else {
    $UseNamedInstance = $false
}

# Step 3: Server Configuration
Write-Title "Step 3 of 7: Database Server Connection"

if ($DatabaseType -eq "SqlServer" -and $UseNamedInstance) {
    Write-Info "Enter the SQL Server named instance in format: SERVERNAME\\INSTANCENAME"
    Write-Info "Examples: localhost\\SQLEXPRESS, .\\DEV, MYSERVER\\INSTANCE1"
    $ServerAddress = Read-Value -Prompt "Named instance" -Required
    Write-Success "Server: $ServerAddress"
    $Port = $null  # Port not used for named instances
} else {
    $ServerAddress = Read-Value -Prompt "Database server address" -Default "localhost"
    Write-Success "Server: $ServerAddress"
    
    $Port = Read-Value -Prompt "Database server port" -Default $defaultPort
    Write-Success "Port: $Port"
}

# Step 4: Database Credentials
Write-Title "Step 4 of 7: Database Administrator Credentials"

Write-Info "These credentials are used to create databases and run migrations."
Write-Info "For SQL Server: typically 'sa' user"
Write-Info "For PostgreSQL: typically 'postgres' user"

$AdminUsername = Read-Value -Prompt "Database admin username" -Default $defaultUsername
Write-Success "Username: $AdminUsername"

$AdminPassword = Read-Value -Prompt "Database admin password" -Required -Secure
Write-Success "Password: (hidden)"

# Step 5: ATSPM Admin Account
Write-Title "Step 5 of 7: ATSPM Admin User"

Write-Info "This will be your first user account to log into ATSPM web interface."
Write-Info ""
Write-Info "Password requirements:"
Write-Info "  - At least 6 characters"
Write-Info "  - At least one lowercase letter (a-z)"
Write-Info "  - At least one uppercase letter (A-Z)"
Write-Info "  - At least one digit (0-9)"
Write-Info "  - At least one special character (!@#$%^&*)"

$AtspmAdminEmail = Read-Value -Prompt "ATSPM admin email" -Default "admin@atspm.local"
Write-Success "Email: $AtspmAdminEmail"

# Validate password strength
do {
    $AtspmAdminPassword = Read-Value -Prompt "ATSPM admin password" -Default "Admin123!" -Required
    
    $validation = Test-PasswordStrength -Password $AtspmAdminPassword
    
    if ($validation.Valid) {
        Write-Success "Password meets requirements"
        break
    } else {
        Write-Host ""
        Write-Error "Password does not meet requirements:"
        foreach ($errorMsg in $validation.Errors) {
            Write-Host "  - $errorMsg" -ForegroundColor Red
        }
        Write-Host ""
        Write-Host "Please try again" -ForegroundColor Yellow
        Write-Host ""
    }
} while ($true)

# Step 6: Confirmation
Write-Title "Step 6 of 7: Review Configuration"

Write-Host "Please review your configuration:" -ForegroundColor White
Write-Host ""
Write-Host "  Database Type         : " -NoNewline; Write-Host $DatabaseType -ForegroundColor Cyan
if ($DatabaseType -eq "SqlServer" -and $UseNamedInstance) {
    Write-Host "  Connection Method     : " -NoNewline; Write-Host "Named Instance" -ForegroundColor Cyan
    Write-Host "  Server Instance       : " -NoNewline; Write-Host $ServerAddress -ForegroundColor Cyan
} else {
    Write-Host "  Connection Method     : " -NoNewline; Write-Host "IP/Hostname + Port" -ForegroundColor Cyan
    Write-Host "  Server Address        : " -NoNewline; Write-Host $ServerAddress -ForegroundColor Cyan
    Write-Host "  Port                  : " -NoNewline; Write-Host $Port -ForegroundColor Cyan
}
Write-Host "  Database Admin User   : " -NoNewline; Write-Host $AdminUsername -ForegroundColor Cyan
Write-Host "  Database Admin Password: " -NoNewline; Write-Host "(hidden)" -ForegroundColor Gray
Write-Host ""
Write-Host "  ATSPM Admin Email     : " -NoNewline; Write-Host $AtspmAdminEmail -ForegroundColor Green
Write-Host "  ATSPM Admin Password  : " -NoNewline; Write-Host $AtspmAdminPassword -ForegroundColor Green
Write-Host ""
Write-Host "What will be created:" -ForegroundColor White
Write-Host "  - ATSPM-Config database" -ForegroundColor Yellow
Write-Host "  - ATSPM-Aggregation database" -ForegroundColor Yellow
Write-Host "  - ATSPM-EventLogs database" -ForegroundColor Yellow
Write-Host "  - ATSPM-Identity database" -ForegroundColor Yellow
Write-Host "  - All tables, reference data, and admin user" -ForegroundColor Yellow
Write-Host "  - Configuration file: ..\Atspm\.env" -ForegroundColor Yellow
Write-Host ""

if (-not (Confirm-Action "Proceed with this configuration?")) {
    Write-Host ""
    Write-Host "Setup cancelled." -ForegroundColor Yellow
    Write-Host ""
    pause
    exit 0
}

# Step 7: Execute Setup
Write-Title "Step 7 of 7: Running Setup"

Write-Host "Please wait while the setup runs. This may take 2-5 minutes..." -ForegroundColor Cyan
Write-Host ""

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

try {
    # Determine template file based on database type
    if ($DatabaseType -eq "SqlServer") {
        $TemplateFile = Join-Path $ScriptDir "templates\.env.external-sqlserver.template"
        $Provider = "SqlServer"
    } else {
        $TemplateFile = Join-Path $ScriptDir "templates\.env.external-postgresql.template"
        $Provider = "PostgreSQL"
    }
    
    if (-not (Test-Path $TemplateFile)) {
        Write-Error "Template file not found: $TemplateFile"
        pause
        exit 1
    }
    
    # Read template
    Write-Host "[*] Generating .env configuration file..." -ForegroundColor Cyan
    $envContent = Get-Content $TemplateFile -Raw
    
    # Replace placeholders based on database type
    if ($DatabaseType -eq "SqlServer") {
        $envContent = $envContent -replace '\$\{SQL_SERVER\}', 'host.docker.internal'
        $envContent = $envContent -replace '\$\{SQL_PORT\}', $Port
        $envContent = $envContent -replace '\$\{SQL_USER\}', $AdminUsername
        $envContent = $envContent -replace '\$\{SQL_PASSWORD\}', $AdminPassword
        $envContent = $envContent -replace 'SQL_SERVER=host\.docker\.internal', "SQL_SERVER=host.docker.internal"
        $envContent = $envContent -replace 'SQL_PORT=1433', "SQL_PORT=$Port"
        $envContent = $envContent -replace 'SQL_USER=sa', "SQL_USER=$AdminUsername"
        $envContent = $envContent -replace 'SQL_PASSWORD=YourStrong!Passw0rd', "SQL_PASSWORD=$AdminPassword"
    } else {
        $envContent = $envContent -replace '\$\{POSTGRES_HOST\}', 'host.docker.internal'
        $envContent = $envContent -replace '\$\{POSTGRES_PORT\}', $Port
        $envContent = $envContent -replace '\$\{POSTGRES_USER\}', $AdminUsername
        $envContent = $envContent -replace '\$\{POSTGRES_PASSWORD\}', $AdminPassword
        $envContent = $envContent -replace 'POSTGRES_HOST=host\.docker\.internal', "POSTGRES_HOST=host.docker.internal"
        $envContent = $envContent -replace 'POSTGRES_PORT=5432', "POSTGRES_PORT=$Port"
        $envContent = $envContent -replace 'POSTGRES_USER=postgres', "POSTGRES_USER=$AdminUsername"
        $envContent = $envContent -replace 'POSTGRES_PASSWORD=YourPostgresPassword', "POSTGRES_PASSWORD=$AdminPassword"
    }
    
    # Replace admin credentials
    $envContent = $envContent -replace 'ADMIN_EMAIL=admin@atspm\.local', "ADMIN_EMAIL=$AtspmAdminEmail"
    $envContent = $envContent -replace 'ADMIN_PASSWORD=Admin123!', "ADMIN_PASSWORD=$AtspmAdminPassword"
    
    # Save .env file
    $EnvFile = Join-Path (Split-Path $ScriptDir -Parent) "Atspm\.env"
    $envContent | Set-Content -Path $EnvFile -Encoding UTF8 -Force
    Write-Host "[OK] Configuration file saved: $EnvFile" -ForegroundColor Green
    Write-Host ""
    
    # Run DatabaseInstaller
    Write-Host "[*] Creating databases and running migrations..." -ForegroundColor Cyan
    $DatabaseInstallerPath = Join-Path (Split-Path $ScriptDir -Parent) "Atspm\DatabaseInstaller"
    
    if (-not (Test-Path $DatabaseInstallerPath)) {
        Write-Error "DatabaseInstaller not found at: $DatabaseInstallerPath"
        pause
        exit 1
    }
    
    Push-Location $DatabaseInstallerPath
    
    # Build connection strings for DatabaseInstaller
    if ($DatabaseType -eq "SqlServer") {
        # For named instances, use just the server\instance name without port
        # For IP+Port, include the port with comma separator
        if ($UseNamedInstance) {
            $ServerPart = "Server=$ServerAddress"
        } else {
            $ServerPart = "Server=$ServerAddress,$Port"
        }
        
        $ConfigConn = "$ServerPart;Database=ATSPM-Config;User Id=$AdminUsername;Password=$AdminPassword;TrustServerCertificate=True;Pooling=true;Timeout=30;Command Timeout=60;"
        $AggregationConn = "$ServerPart;Database=ATSPM-Aggregation;User Id=$AdminUsername;Password=$AdminPassword;TrustServerCertificate=True;Pooling=true;Timeout=30;Command Timeout=60;"
        $EventLogConn = "$ServerPart;Database=ATSPM-EventLogs;User Id=$AdminUsername;Password=$AdminPassword;TrustServerCertificate=True;Pooling=true;Timeout=30;Command Timeout=60;"
        $IdentityConn = "$ServerPart;Database=ATSPM-Identity;User Id=$AdminUsername;Password=$AdminPassword;TrustServerCertificate=True;Pooling=true;Timeout=30;Command Timeout=60;"
    } else {
        $ConfigConn = "Host=$ServerAddress;Port=$Port;Database=ATSPM-Config;Username=$AdminUsername;Password=$AdminPassword;"
        $AggregationConn = "Host=$ServerAddress;Port=$Port;Database=ATSPM-Aggregation;Username=$AdminUsername;Password=$AdminPassword;"
        $EventLogConn = "Host=$ServerAddress;Port=$Port;Database=ATSPM-EventLogs;Username=$AdminUsername;Password=$AdminPassword;"
        $IdentityConn = "Host=$ServerAddress;Port=$Port;Database=ATSPM-Identity;Username=$AdminUsername;Password=$AdminPassword;"
    }
    
    # Run DatabaseInstaller with connection strings as command-line arguments
    Write-Host "[*] Running DatabaseInstaller with provider: $Provider" -ForegroundColor Cyan
    Write-Host ""
    
    $InstallerArgs = @(
        "run",
        "--",
        "update",
        "--provider", $Provider,
        "--config-connection", $ConfigConn,
        "--aggregation-connection", $AggregationConn,
        "--eventlog-connection", $EventLogConn,
        "--identity-connection", $IdentityConn,
        "--admin-email", $AtspmAdminEmail,
        "--admin-password", $AtspmAdminPassword,
        "--admin-role", "Admin",
        "--seed-admin", "true"
    )
    
    # Run dotnet command and display output in real-time
    & dotnet $InstallerArgs
    $exitCode = $LASTEXITCODE
    
    Pop-Location
    
    Write-Host ""
    
    # Check for errors
    if ($exitCode -ne 0) {
        Write-Host ""
        Write-Error "DatabaseInstaller failed with exit code: $exitCode"
        Write-Host ""
        Write-Host "Common issues:" -ForegroundColor Yellow
        Write-Host "  - Cannot connect to database server (check server address, port, and credentials)" -ForegroundColor Gray
        Write-Host "  - Databases don't exist yet (you may need to create them manually first)" -ForegroundColor Gray
        Write-Host "  - Network/firewall blocking connection" -ForegroundColor Gray
        Write-Host "  - SQL Server Browser service not running (for named instances)" -ForegroundColor Gray
        Write-Host ""
        pause
        exit 1
    }
    
    Write-Host "[OK] Database setup completed successfully" -ForegroundColor Green
    
    Write-Host ""
    Write-Title "Setup Complete!"
    
    Write-Host "[OK] Databases created and configured" -ForegroundColor Green
    Write-Host "[OK] Tables and schemas created" -ForegroundColor Green
    Write-Host "[OK] Reference data seeded" -ForegroundColor Green
    Write-Host "[OK] Admin user created" -ForegroundColor Green
    Write-Host "[OK] Configuration file generated" -ForegroundColor Green
    Write-Host ""
    Write-Host "==========================================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Next Steps:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "  1. Review the configuration file:" -ForegroundColor White
    Write-Host "     ..\Atspm\.env" -ForegroundColor Gray
    Write-Host ""
    Write-Host "  2. Start ATSPM with Docker Compose:" -ForegroundColor White
    Write-Host "     cd ..\Atspm" -ForegroundColor Gray
    Write-Host "     docker-compose -f docker-compose.external-db.yml up --build" -ForegroundColor Gray
    Write-Host ""
    Write-Host "  3. Access ATSPM Web UI:" -ForegroundColor White
    Write-Host "     https://localhost/" -ForegroundColor Gray
    Write-Host ""
    Write-Host "  4. Log in with your admin credentials:" -ForegroundColor White
    Write-Host "     Email   : $AtspmAdminEmail" -ForegroundColor Gray
    Write-Host "     Password: $AtspmAdminPassword" -ForegroundColor Gray
    Write-Host ""
    Write-Host "==========================================================================" -ForegroundColor Cyan
    Write-Host ""
    
} catch {
    Write-Host ""
    Write-Title "Setup Failed"
    Write-Error $_.Exception.Message
    Write-Host ""
    Write-Host "Error Details:" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    Write-Host ""
}

Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
