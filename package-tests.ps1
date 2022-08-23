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
Copy-Item -Path ./build/Debug/Duster.TestPlugin/net6.0/ -Destination ./build/App/mods/test-plugin/ -Recurse

Write-Output "Copying RendererPlugin."
Copy-Item -Path ./build/Debug/Duster.Renderer/net6.0/ -Destination ./build/App/mods/render-plugin/ -Recurse

# Write-Output "Copying to integration tests"
# Copy-Item -Path ./build/Debug/Duster.App/net6.0/* -Destination ./tests/integration-tests/App -Recurse
# Copy-Item -Path ./build/Debug/Duster.TestPlugin/net6.0/ -Destination ./tests/integration-tests/App/mods/test-plugin/ -Recurse
# Copy-Item -Path ./build/Debug/Duster.Renderer/net6.0/ -Destination ./tests/integration-tests/App/mods/render-plugin/ -Recurse
