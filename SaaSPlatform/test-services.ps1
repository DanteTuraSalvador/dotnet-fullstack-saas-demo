# Test SaaS Platform Services
Write-Host "=== Testing SaaS Platform Services ==="
Write-Host ""

# Test API Service
Write-Host "1. Testing API Service (http://localhost:5153)..."
try {
    $apiResponse = Invoke-WebRequest -Uri "http://localhost:5153/api/subscriptions" -Method GET -TimeoutSec 10
    Write-Host "   Status: Success"
    Write-Host "   Status Code: $($apiResponse.StatusCode)"
    Write-Host "   Content Length: $($apiResponse.Content.Length) characters"
} catch {
    Write-Host "   Status: Failed"
    Write-Host "   Error: $($_.Exception.Message)"
}
Write-Host ""

# Test Client Web Service
Write-Host "2. Testing Client Web Service (http://localhost:5000)..."
try {
    $clientResponse = Invoke-WebRequest -Uri "http://localhost:5000" -Method GET -TimeoutSec 10
    Write-Host "   Status: Success"
    Write-Host "   Status Code: $($clientResponse.StatusCode)"
    Write-Host "   Content Length: $($clientResponse.Content.Length) characters"
} catch {
    Write-Host "   Status: Failed"
    Write-Host "   Error: $($_.Exception.Message)"
}
Write-Host ""

# Test Admin Web Service
Write-Host "3. Testing Admin Web Service (http://localhost:5001)..."
try {
    $adminResponse = Invoke-WebRequest -Uri "http://localhost:5001" -Method GET -TimeoutSec 10
    Write-Host "   Status: Success"
    Write-Host "   Status Code: $($adminResponse.StatusCode)"
    Write-Host "   Content Length: $($adminResponse.Content.Length) characters"
} catch {
    Write-Host "   Status: Failed"
    Write-Host "   Error: $($_.Exception.Message)"
}
Write-Host ""

# Test SQL Server
Write-Host "4. Testing SQL Server Connection (localhost:1433)..."
try {
    $tcpClient = New-Object System.Net.Sockets.TcpClient
    $tcpClient.Connect("localhost", 1433)
    if ($tcpClient.Connected) {
        Write-Host "   Status: Success"
        Write-Host "   Connection: Established"
    } else {
        Write-Host "   Status: Failed"
        Write-Host "   Connection: Not established"
    }
    $tcpClient.Close()
} catch {
    Write-Host "   Status: Failed"
    Write-Host "   Error: $($_.Exception.Message)"
}
Write-Host ""

Write-Host "=== Test Complete ==="