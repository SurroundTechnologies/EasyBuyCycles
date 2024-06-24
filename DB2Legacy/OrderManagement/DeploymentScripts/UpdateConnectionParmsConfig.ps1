param($path, $serverid, $key, $value)

$xml = [xml](Get-Content $path)

$matchingNode = $xml.configuration.sqlSettings.server |
    where {$_.id -eq $serverid}

$matchingNode = $matchingNode.add |
    where {$_.key -eq $key}

$matchingNode.value = $value

$xml.Save($path)
