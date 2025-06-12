$shipments = @(
    @{  Id= "00000000-0000-a000-0000-000000000001" },
    @{  Id= "00000000-0000-a000-0000-000000000006" },
    @{  Id= "00000000-0000-a000-0000-000000000005" }
)

foreach ($shipment in $shipments) {
    Write-Output "Ship oder for: $($order.Id)"

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")

$body = @{
            orderId              = $shipment.Id  
            } | ConvertTo-Json

$random = Get-Random -Maximum 5
Start-Sleep -Seconds $random
Invoke-RestMethod 'http://localhost:5080/orders/ship' -Method 'POST' -Headers $headers -Body $body
Write-Output "Order shipped"
}
