using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.Windowing;
using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

namespace Duster.Renderer;

public struct DrawInfo
{
    public int X;
}

public class WindowSystem : ISystem<float>
{
    private IWindow _window;
    private IInputContext _input;
    private World _world;
    public bool IsEnabled { get; set; }

    public WindowSystem(World world)
    {
        _world = world;

        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1920, 1080);
        options.Title = "Hello world from plugin from silk";
        options.IsEventDriven = false;

        Window.PrioritizeSdl();
        _window = Window.Create(options);
        _window.Initialize();
        _input = _window.CreateInput();
    }

    public void Events()
    {
        foreach (var kb in _input.Keyboards)
        {
            kb.KeyDown += (kb, k, mod) =>
            {
                if (k == Key.Escape)
                {
                    _world.Get<ApplicationState>().Quit = true;
                    _window.Close();
                }
            };
        }
    }
    public void Draw() { }

    public void Update(float state)
    {
        _window.DoEvents();
        Events();
        Draw();
        _window.DoRender();
    }

    public void Dispose() { }
}