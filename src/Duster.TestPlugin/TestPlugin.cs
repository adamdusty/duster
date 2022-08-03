﻿using Duster.Sdk;

using static SDL2.SDL;

namespace Duster.TestPlugin;
public class TestPlugin : IPlugin
{
    public string Name => "Test Plugin";
    public Sdk.Version Version => new Sdk.Version(0, 1, 0);

    public void Load()
    {
        System.Console.WriteLine("TestPlugin Loading");
    }

    public void Unload()
    {
        System.Console.WriteLine("TestPlugin Unloading");
    }
}

public static class Library
{
    public static string Greet()
    {
        return "Hello from TestPugin";
    }
}