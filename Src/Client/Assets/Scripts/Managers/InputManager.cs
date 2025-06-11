using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Models;
using SkillBridge.Message;

namespace Managers
{
    class InputManager : MonoSingleton<InputManager>
    {
        public InputManager() { }

        public bool IsInputMode = false;
    }
}