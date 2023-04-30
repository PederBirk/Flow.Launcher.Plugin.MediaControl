using System.Collections;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace Flow.Launcher.Plugin.MediaControl;

public enum MediaPlaybackAction
{
    Toggle,
    Next,
    Previous,
    Stop
}

public static class MediaController
{
    public static void Execute(MediaPlaybackAction action)
    {
        var key = action switch
        {
            MediaPlaybackAction.Next => VIRTUAL_KEY.VK_MEDIA_NEXT_TRACK,
            MediaPlaybackAction.Toggle => VIRTUAL_KEY.VK_MEDIA_PLAY_PAUSE,
            MediaPlaybackAction.Previous => VIRTUAL_KEY.VK_MEDIA_PREV_TRACK,
            MediaPlaybackAction.Stop => VIRTUAL_KEY.VK_MEDIA_STOP,
            _ => throw new System.Exception("Invalid action")
        };

        var inputs = new INPUT[]{
            new () {
                type = INPUT_TYPE.INPUT_KEYBOARD,
                Anonymous = new()
                {
                    ki = new()
                    {
                        wVk = key
                    }
                }
            },  new ()
            {
                type = INPUT_TYPE.INPUT_KEYBOARD,
                Anonymous = new()
                {
                    ki = new()
                    {
                        wVk = key,
                        dwFlags = KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP
                    }
                }
            }
        };

        PInvoke.SendInput(inputs, Marshal.SizeOf<INPUT>());
    }
}