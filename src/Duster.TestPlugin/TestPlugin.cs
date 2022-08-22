using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Duster.Sdk;
using Duster.Renderer;

namespace Duster.TestPlugin;

public class TestPlugin : IPlugin
{
    public DrawInfo DrawInfo { get; set; }

    public void Initialize()
    {
        DrawInfo = new DrawInfo { X = 42 };
        System.Console.WriteLine(DrawInfo.X);
    }
}