$orders = @(
    @{ Name = "Lucek"; Address = "PL"; Id= "00000000-0000-a000-0000-000000000001"; Red= 10; Blue = 15; Green = 0  },
    @{ Name = "Andrew"; Address = "UK"; Id= "00000000-0000-a000-0000-000000000002"; Red= 9; Blue = 0; Green = 11  },
    @{ Name = "Vlad"; Address = "RU"; Id= "00000000-0000-a000-0000-000000000003"; Red= 6; Blue = 8; Green = 13  },
    @{ Name = "Lucek"; Address = "PL"; Id= "00000000-0000-a000-0000-000000000004"; Red= 34; Blue = 91; Green = 51  },
    @{ Name = "Lucek"; Address = "ESP"; Id= "00000000-0000-a000-0000-000000000005"; Red= 21; Blue = 45; Green = 7  },
    @{ Name = "Lucek"; Address = "LIT"; Id= "00000000-0000-a000-0000-000000000006"; Red= 1; Blue = 33; Green = 5  },
    @{ Name = "Lucek"; Address = "FIN"; Id= "00000000-0000-a000-0000-000000000007"; Red= 6; Blue = 1; Green = 12  }
)

foreach ($order in $orders) {
    Write-Output "Send oder for: $($order.Name)"

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")

$body = @{
                id              = $order.Id
                customerName    = $order.Name
                shippingAddress = $order.Address
                items            = @{
                    red          = $order.Red
                    green = $order.Green
                    blue     = $order.Blue
                }
            } | ConvertTo-Json -depth 2

Invoke-RestMethod 'http://localhost:5080/orders/add' -Method 'POST' -Headers $headers -Body $body
Write-Output "Order sent"
}
