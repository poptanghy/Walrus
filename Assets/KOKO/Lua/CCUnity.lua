local _G = _G
local CC = require("CC")

module "CCUnity"
CCUnity = {}
function CCUnity:SendWeb(kURL, tVal, kCallBack, kCallBack2) 
    tVal.appKey = tVal.appKey or Player.appKey
    tVal.uid = tVal.uid or Player.uid
    function LF_CallBack(tVal)
        Print("Web >> "..kURL..Table:ToString(tVal))
        if kCallBack then
            if kCallBack2 then
                UIMgr:Do(kCallBack, kCallBack2, tVal)
            else
                kCallBack(tVal)
            end
        else
            Message(tVal.success and {Tip = tVal.message} or {Text = tVal.message})
        end
    end
    Print("ClientW << "..kURL..Table:ToString(tVal))
    kk.ql.httpPost(require("config").webHost .. kURL, tVal, LF_CallBack)
end