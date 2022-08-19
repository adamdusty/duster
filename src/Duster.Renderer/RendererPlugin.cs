using DefaultEcs;
using DefaultEcs.System;
using Duster.Sdk;

using static SDL2.SDL;

namespace Duster.Renderer;
public class RendererPlugin : IPlugin
{
    private SDL_Event _event;
    private IntPtr _window;
    private IntPtr _renderer;
    private World? _world;

    public string Name => "Renderer";

    public string Description => "Core renderer.";

    public List<ISystem<float>> FrameUpdateSystems { get; private set; }

    public List<ISystem<float>> FixedUpdateSystems { get; private set; }

    public RendererPlugin()
    {
        FrameUpdateSystems = new List<ISystem<float>>();
        FixedUpdateSystems = new List<ISystem<float>>();
    }

    private void SignalQuit()
    {
        if (_world is not null)
            _world.Get<ApplicationState>().Quit = true;
    }

    public void Initialize(World world)
    {
        _world = world;
        SDL_Init(SDL_INIT_VIDEO);
        _window = SDL_CreateWindow(
            "RenderPlugin window",
            SDL_WINDOWPOS_CENTERED,
            SDL_WINDOWPOS_CENTERED,
            1920,
            1080,
            SDL_WindowFlags.SDL_WINDOW_SHOWN
        );

        _renderer = SDL_CreateRenderer(_window, 0, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

        FrameUpdateSystems.Add(new ActionSystem<float>(s =>
        {
            Input();

            SDL_SetRenderDrawColor(_renderer, 255, 0, 0, 255);
            SDL_RenderClear(_renderer);
            SDL_RenderPresent(_renderer);
        }));


    }

    private void Input()
    {
        while (SDL_PollEvent(out _event) != 0)
        {
            switch (_event.type)
            {
                case SDL_EventType.SDL_QUIT:
                    SignalQuit();
                    break;
                case SDL_EventType.SDL_KEYDOWN:
                    switch (_event.key.keysym.scancode)
                    {
                        case SDL_Scancode.SDL_SCANCODE_ESCAPE:
                            SignalQuit();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void Dispose()
    {
        SDL_DestroyWindow(_window);
        SDL_Quit();
    }
}
