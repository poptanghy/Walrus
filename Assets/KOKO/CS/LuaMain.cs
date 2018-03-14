using UnityEngine;
using System.Collections;
using LuaInterface;

namespace CC
{
    //展示searchpath 使用，require 与 dofile 区别
    public class LuaMain : LuaClient
    {
        new public static LuaMain Instance;
        protected override LuaFileUtils InitLoader()
        {
            return new LuaResLoader();
        }
        protected override void OpenLibs()
        {
            base.OpenLibs();
            OpenCJson();
        }
        protected override void OnLoadFinished()
        {
            Instance = this;
            Application.logMessageReceived += ShowTips;

            luaState.Start();
            string fullPath = Application.dataPath + "\\KOKO/Lua";
            luaState.AddSearchPath(fullPath);
            luaState.Require("LuaMain");
            Net.SendWeb();
        }

        string tips;
        void ShowTips(string msg, string stackTrace, LogType type)
        {
            tips += msg;
            tips += "\r\n";
        }

        public void Call_String(string kFunc, string kTemp)
        {
            LuaFunction luaFunc = luaState.GetFunction(kFunc);
            if (luaFunc != null)
            {
                luaFunc.Call<string>(kTemp);
            }
        }

        new void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            Application.logMessageReceived -= ShowTips;
        }
        void OnGUI()
        {
            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 600), tips);
        }
    }


}