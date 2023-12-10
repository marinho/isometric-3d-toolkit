using Godot;
using System;

namespace Isometric3DEngine
{
    public partial class VisibleAndProcessTogether : Node3D
    {
        [Signal]
        public delegate void BeforeShowEventHandler();

        [Signal]
        public delegate void AfterShowEventHandler();

        [Signal]
        public delegate void BeforeHideEventHandler();

        [Signal]
        public delegate void AfterHideEventHandler();

        public enum EventHandler
        {
            BeforeShow,
            AfterShow,
            BeforeHide,
            AfterHide,
        }

        public void ShowAndEnable()
        {
            EmitSignal(EventHandler.BeforeShow.ToString());
            Show();
            ProcessMode = ProcessModeEnum.Inherit;
            EmitSignal(EventHandler.AfterShow.ToString());
        }

        public void HideAndDisable()
        {
            EmitSignal(EventHandler.BeforeHide.ToString());
            Hide();
            ProcessMode = ProcessModeEnum.Disabled;
            EmitSignal(EventHandler.AfterHide.ToString());
        }
    }
}
