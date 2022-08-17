dotnet build -c Debug

if (-Not ($LASTEXITCODE -eq 0)) {
    return
}

Write-Output "Build Succeeded. Packaing tests."

if (Test-Path ./build/App -PathType Container) {
    Write-Output "Removing old App directory."
    Remove-Item ./build/App -Recurse -Force
}

New-Item -ItemType Directory -Force -Path ./build/App | Out-Null

Write-Output "Copying App."
Copy-Item -Path ./build/Debug/Duster.App/net6.0/* -Destination ./build/App -Recurse

Write-Output "Copying TestPlugin."
Copy-Item -Path ./build/Debug/Duster.TestPlugin/net6.0/ -Destination ./build/App/plugins/test-plugin/ -Recurse