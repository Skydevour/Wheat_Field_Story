using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CommonFramework.Editor.GuiBehaviour
{
    public abstract class ButtonGuiRenderBase : GuiRenderBase
    {
        protected GUIContent guiContent;
        private Action onButtonPressed;

        public ButtonGuiRenderBase(Action onButtonPressedCallback)
        {
            this.onButtonPressed = onButtonPressedCallback;
        }

        public override void RenderGui()
        {
            if (GUILayout.Button(guiContent))
            {
                onButtonPressed?.Invoke();
            }
        }
    }
}
