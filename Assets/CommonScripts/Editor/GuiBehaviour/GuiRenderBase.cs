using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CommonFramework.Editor.GuiBehaviour
{
    public abstract class GuiRenderBase
    {
        public abstract long ModeMask { get; }

        /// <summary>
        /// Other actions which will block this actions from starting if it's not running
        /// </summary>
        public abstract long BlockingModes { get; }

        /// <summary>
        /// Other actions which was required by this action, in order for this action to be executed
        /// Note that this action will be deactivate if required actions were killed
        /// </summary>
        public abstract long RequiredModes { get; }

        public virtual KeyCode SpecialKey { get { return KeyCode.None; } }

        /// <summary>
        /// Hot key to enable certain mode
        /// </summary>
        public virtual KeyCode HotKey { get { return KeyCode.None; } } //TODO: Change to multiple key bindings if necessary

        public virtual void RenderGui() { }

        public virtual bool CanBeEnabled() { return true; }

        public virtual void HandleGuiEvent(Event currentEvent) { }
    }
}
