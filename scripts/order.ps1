$orders = @(
    @{ Name = "Reksio"; Address = "PL"; Longitude ="23.473761"; Latitude="51.132648"; Id= "00000000-0000-a000-0000-000000000001"; Red= 10; Blue = 15; Green = 0  },
    @{ Name = "Postman Pat"; Address = "UK"; Longitude ="-0.127758"; Latitude="51.507351"; Id= "00000000-0000-a000-0000-000000000002"; Red= 9; Blue = 0; Green = 11  },
    @{ Name = "Fireman Sam"; Address = "GER"; Longitude ="13.404954"; Latitude="52.520008"; Id= "00000000-0000-a000-0000-000000000003"; Red= 6; Blue = 8; Green = 13  },
    @{ Name = "Papa Smurf"; Address = "BE"; Longitude ="4.351697"; Latitude="50.8465573"; Id= "00000000-0000-a000-0000-000000000004"; Red= 34; Blue = 91; Green = 51  },
    @{ Name = "Pat&Mat"; Address = "SL";Longitude ="17.1314203"; Latitude="48.1557669"; Id= "00000000-0000-a000-0000-000000000005"; Red= 21; Blue = 45; Green = 7  },
    @{ Name = "Pingu"; Address = "CH";Longitude ="6.1466014"; Latitude="46.2017559"; Id= "00000000-0000-a000-0000-000000000006"; Red= 1; Blue = 33; Green = 5  },
    @{ Name = "Moominmamma"; Address = "FIN";Longitude ="24.9427473"; Latitude="60.1674881"; Id= "00000000-0000-a000-0000-000000000007"; Red= 34; Blue = 23; Green = 96  },
    @{ Name = "Panoramix"; Address = "FRA";Longitude ="2.320041"; Latitude="48.8588897"; Id= "00000000-0000-a000-0000-000000000008"; Red= 45; Blue = 1; Green = 1  },
    @{ Name = "Krtek"; Address = "CZ";Longitude ="14.4212535"; Latitude="50.0874654"; Id= "00000000-0000-a000-0000-000000000009"; Red= 3; Blue = 13; Green = 23  },
    @{ Name = "Pinokio"; Address = "ITA";Longitude ="12.4829321"; Latitude="41.8933203"; Id= "00000000-0000-a000-0000-000000000010"; Red= 24; Blue = 34; Green = 7  },
    @{ Name = "Filemon"; Address = "PL";Longitude ="21.0067249"; Latitude="52.2319581"; Id= "00000000-0000-a000-0000-000000000011"; Red= 32; Blue = 25; Green = 17  },
    @{ Name = "Uszatek"; Address = "PL";Longitude ="18.6129831"; Latitude="54.3706858"; Id= "00000000-0000-a000-0000-000000000012"; Red= 6; Blue = 1; Green = 13  },
    @{ Name = "captain Bomba"; Address = "PL";Longitude ="21.1541546"; Latitude="51.4022557"; Id= "00000000-0000-a000-0000-000000000013"; Red= 1; Blue = 14; Green = 23  },
    @{ Name = "Padington"; Address = "UK";Longitude ="-2.2451148"; Latitude="53.4794892"; Id= "00000000-0000-a000-0000-000000000014"; Red= 21; Blue = 15; Green = 3  },
    @{ Name = "Thomas"; Address = "UK";Longitude ="-2.99168"; Latitude="53.4071991"; Id= "00000000-0000-a000-0000-000000000015"; Red= 6; Blue = 1; Green = 2  }
)

foreach ($order in $orders) {
    Write-Output "Send oder for: $($order.Name)"

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")

$body = @{
                id              = $order.Id
                customerName    = $order.Name
                shippingAddress = $order.Address
                latitude        = $order.Latitude
                Longitude       = $order.Longitude
                items            = @{
                    red          = $order.Red
                    green = $order.Green
                    blue     = $order.Blue
                }
            } | ConvertTo-Json -depth 2
$random = Get-Random -Maximum 25
Start-Sleep -Duration (New-TimeSpan -Seconds $random)
Invoke-RestMethod 'http://localhost:5080/orders/add' -Method 'POST' -Headers $headers -Body $body
Write-Output "Order sent"
}
