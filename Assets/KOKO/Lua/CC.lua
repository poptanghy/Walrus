module ("CC", package.seeall)
----------------------------------------------------------------
-- Menu
----------------------------------------------------------------
-- Functions

----------------------------------------------------------------



-- Functions
----------------------------------------------------------------
ToString = tostring
Assert = assert
function Print( ... )
    print(os.date(),"= CC =", ...)
end
function ToBool(kVal)
    if kVal then
        return true
    else
        return false
    end
end

function Copy(kOld, tLookup)
    tLookup = tLookup or {}
    if _G.type(kOld) ~= "table" then
        return kOld
    elseif tLookup[kOld] then
        return tLookup[kOld]
    else        
        local tNew = {}
        tLookup[kOld] = tNew
        for i,v in _G.pairs(kOld) do
            tNew[Copy(i)] = Copy(v)
        end  
        return _G.setmetatable(tNew, Copy(_G.getmetatable(kOld)))
    end 
end

function SimpleCopy(tVal)
    if type(tVal) == "table" then
        local tRt = {}
        for k,v in pairs(tVal) do
            tRt[k] = v
        end
        _G.setmetatable(tRt, _G.getmetatable(tVal))
        return tRt
    else
        return tVal
    end
end
-----------------------------------------------------------------------------------
function SimpleUpgrade(tOld, tAdd)
    tOld = tOld or {}
    if tAdd then
        for k,v in _G.pairs(tAdd) do
            tOld[k] = v
        end
    end
    return tOld
end
-----------------------------------------------------------------------------------
function Upgrade(tOld, tAdd)
    tOld = tOld or {}
    local tLookup = {}
    if tAdd then
        for k,v in _G.pairs(tAdd) do
            tOld[Copy(k, tLookup)] = Copy(v, tLookup)
        end
    end
    return tOld
end
-----------------------------------------------------------------------------------
function Class(tVal, kName, ... )
    local kClass = class(kName, ...)
    kClass.__tVal = tVal
    kClass.__kName = kName
    function kClass:__New( ... )
        local kTemp = kClass.new(...)
        if kTemp.__supers then
            for i,v in ipairs(kTemp.__supers) do
                Upgrade(kTemp, v.__tVal)
            end
        end
        Upgrade(kTemp, self.__tVal)
        return kTemp
    end

    local bCreate = false
    for k,v in pairs({...}) do
        if v.Create ~= v.__New then
            bCreate = true
        end
    end
    if not bCreate then
        kClass.Create = kClass.__New
    end
    return kClass
end
-----------------------------------------------------------------------------------
function Switch()
    local kClass = {__bOn = false}
    function kClass:TryOn()
        if not self.__bOn then
            self.__bOn = true
            return true
        end
        return false
    end
    function kClass:TryOff()
        if self.__bOn then
            self.__bOn = false
            return true
        end
        return false
    end
    function kClass:IsOn()
        return self.__bOn
    end
    function kClass:Toggle()
        self.__bOn = not self.__bOn
    end
    return kClass
end
-----------------------------------------------------------------------------------
function Step(i, tStep)
    i = i or 999999
    if tStep then
        tStep[0] = "Null"
    end
    local kClass = {__iStep = i, __iMax = i}
    function kClass:Load()
        self.__tStep = Table:Inverse(tStep)
    end
    function kClass:Try(i)
        i = self:__Change(i)
        if self.__iStep + 1 == i or (i == 1 and self.__iStep == self.__iMax) then
            self.__iStep = i
            return true
        end
        return false
    end
    function kClass:Jump(i)
        i = self:__Change(i)
        self.__iStep = i
    end
    function kClass:Is(i)
        i = self:__Change(i)
        i = i == 0 and self.__iMax or i
        return self.__iStep == i
    end
    function kClass:Get()
        return self.__iStep
    end
    function kClass:__Change(i)
        if not self.__bInit then
            self.__bInit = true
            self:Load()
        end
        return self.__tStep and type(i) == "string" and self.__tStep[i] or i
    end
    return kClass
end
-----------------------------------------------------------------------------------
function Function(kClass, kFunc)
    local kClassTemp, kFuncTemp = kClass, kFunc
    function LF_Action(...)
        return kFuncTemp(kClassTemp, ...)
    end
    return LF_Action
end
-----------------------------------------------------------------------------------
function ClassFunction(kClass, kFunc)
    if kFunc then
        return ClassFunction(Function(kClass, kFunc))
    end
    local kFuncTemp = kClass
    function LF_Action(kClass, ...)
        return kFuncTemp(...)
    end
    return LF_Action
end
