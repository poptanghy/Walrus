using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;

namespace CC
{
    //展示searchpath 使用，require 与 dofile 区别
    public class LuaMain : MonoBehaviour
    {
        LuaState lua = null;
        private string strLog = "";

        void Start()
        {
            Application.logMessageReceived += Log;
            lua = new LuaState();
            lua.Start();
            string fullPath = Application.dataPath + "\\KOKO/Lua";
            lua.AddSearchPath(fullPath);
            lua.Require("LuaMain");
        }

        void Log(string msg, string stackTrace, LogType type)
        {
            //print("== Lua ==" + msg);
        }

        void OnApplicationQuit()
        {
            lua.Dispose();
            lua = null;
            Application.logMessageReceived -= Log;
        }
    }


}