using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CommonFramework.Editor.GuiBehaviour
{
    public abstract class GuiBehaviourBase
    {
        protected Action<long> onHotKeyPressed;
        protected GuiRenderBase[] guiComponents;
        protected readonly long dummyBitsDefinition = 1 << 63;

        protected abstract long CurrentEditorMode { get; }

        private bool suspendHotKeyDetection = false;

        public GuiBehaviourBase(Action<long> onHotKeyPressedCallback, GuiRenderBase[] components)
        {
            guiComponents = components;
            onHotKeyPressed = onHotKeyPressedCallback;
        }

        public void HandleGuiEvents()
        {
            Event currentEvent = Event.current;

            if (currentEvent != null)
            {
                HandleGuiEvents(currentEvent);
            }
        }

        public void HandleHotKeys()
        {
            Event currentEvent = Event.current;

            if (currentEvent.isMouse && currentEvent.button == 1)
            {
                if (currentEvent.type == EventType.MouseDown && !suspendHotKeyDetection)
                {
                    suspendHotKeyDetection = true;
                }
                else if (currentEvent.type == EventType.MouseUp && suspendHotKeyDetection)
                {
                    suspendHotKeyDetection = false;
                }
            }

            if (currentEvent != null && currentEvent.keyCode != KeyCode.None && currentEvent.type == EventType.KeyDown && !suspendHotKeyDetection)
            {
                HandleHotKeys(currentEvent);
            }
        }

        public void RenderInspectorGui()
        {
            for (int i = 0; i < guiComponents.Length; i++)
            {
                GUI.enabled = CanBeEnabled(guiComponents[i]);
                guiComponents[i].RenderGui();
            }
        }

        private void HandleGuiEvents(Event currentEvent)
        {
            for (int i = 0; i < guiComponents.Length; i++)
            {
                if ((guiComponents[i].ModeMask & CurrentEditorMode) != 0)
                {
                    guiComponents[i].HandleGuiEvent(currentEvent);
                }

                //reserve last bit as dummy bit execute without any issues
                //This is a work around since the way IMGUI write was by sequence instead of by states
                if ((guiComponents[i].ModeMask & dummyBitsDefinition) != 0)
                {
                    guiComponents[i].HandleGuiEvent(currentEvent);
                }
            }
        }

        private void HandleHotKeys(Event currentEvent)
        {
            for (int i = 0; i < guiComponents.Length; i++)
            {
                //Ignore dummy bits for hot key events
                if ((guiComponents[i].ModeMask & dummyBitsDefinition) != 0)
                    continue;

                if (guiComponents[i].HotKey == KeyCode.None)
                    continue;

                if (guiComponents[i].HotKey != currentEvent.keyCode)
                    continue;

                if (guiComponents[i].SpecialKey == KeyCode.None
                    && currentEvent.control)
                    continue;

                if ((guiComponents[i].SpecialKey == KeyCode.LeftControl
                    || guiComponents[i].SpecialKey == KeyCode.LeftControl)
                    && !currentEvent.control)
                    continue;

                if (guiComponents[i].SpecialKey != KeyCode.None
                    && guiComponents[i].SpecialKey != KeyCode.LeftControl
                    && guiComponents[i].SpecialKey != KeyCode.LeftControl)
                {
                    Debug.LogErrorFormat("gui componet {0} set wrong special key binding {1}, please fix it", i, guiComponents[i].SpecialKey);
                    continue;
                }


                if (CanBeEnabled(guiComponents[i]))
                {
                    onHotKeyPressed.Invoke(guiComponents[i].ModeMask);
                    return;
                }
            }
        }

        /// <summary>
        /// Check if given action can be activate through all conditions
        /// </summary>
        /// <param name="action">Bitmask action to be checked</param>
        /// <returns>Return true if can be activated</returns>
        protected virtual bool CanBeEnabled(GuiRenderBase action)
        {
            //always enable dummy bits
            if ((action.ModeMask & dummyBitsDefinition) != 0)
                return true;

            return !IsBlocked(action) && HasAnyRequiredModes(action) && action.CanBeEnabled();
        }


        /// <summary>
        /// Check if the dependent actions were running
        /// </summary>
        /// <param name="action">Given action to be checked</param>
        /// <returns>Return true if there is no need for other dependent action, or all required actions were all up and running</returns>
        private bool HasAnyRequiredModes(GuiRenderBase action)
        {
            if (action.RequiredModes == 0) //no need to check this if we dont need to depend on any required actions
                return true;

            if ((action.RequiredModes & CurrentEditorMode) != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if the given action was blocked by the other actions that were already running
        /// </summary>
        /// <param name="action">Action to be checked</param>
        /// <returns>Return true if other blocking action were already running</returns>
        private bool IsBlocked(GuiRenderBase action)
        {
            if ((action.BlockingModes & CurrentEditorMode) != 0)
                return true;
            else
                return false;
        }



    }
}
