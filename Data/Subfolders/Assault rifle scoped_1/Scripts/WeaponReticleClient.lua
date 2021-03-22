--[[
Copyright 2019 Manticore Games, Inc. 

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
--]]

-- Internal custom properties
local WEAPON = script:FindAncestorByType('Weapon')
if not WEAPON:IsA('Weapon') then
    error(script.name .. " should be part of Weapon object hierarchy.")
end

-- Exposed variables
local HIDE_ON_AIM = WEAPON:GetCustomProperty("HideReticleOnAim")
local RETICLE_TEMPLATE = WEAPON:GetCustomProperty("ReticleTemplate")

-- Constant variables
local LOCAL_PLAYER = Game.GetLocalPlayer()

-- Internal variables
local reticleInstance = nil
local isSpawned = false
local isAiming = false
local color = Color.New(1, 1, 1, 0)
local parts

function Approach(a, b, c)
    if a < b then
        a = a + c
    end
    if a > b then
        a = a - c
    end
    return a
end

function Tick()

    -- Check if the owner of the weapon is the local player
    if Object.IsValid(WEAPON) and WEAPON.owner == LOCAL_PLAYER then

        -- Connect with equipped event in case it was not registered
        if not isSpawned then
            -- Spawn reticle only on local player
            SpawnReticle()
            isSpawned = true
        end

        -- Reticle visibility is based on player's health status
        if (Object.IsValid(reticleInstance)) then
            if parts == nil then
                parts = reticleInstance:FindDescendantsByName("Segment")
            elseif not Object.IsValid(parts[1]) then
                parts = reticleInstance:FindDescendantsByName("Segment")
            end
            if HIDE_ON_AIM and isAiming then
                color.a = Approach(color.a, 0, math.abs(color.a)/3)
            else
                if LOCAL_PLAYER.isDead then
                    color.a = Approach(color.a, 0, math.abs(color.a)/4)
                else
                    color.a = Approach(color.a, 1, math.abs(color.a - 1)/6)
                end
            end
            for i = 1, #parts do
                parts[i]:SetColor(color)
            end
        end
    end
end

function SpawnReticle()
    if RETICLE_TEMPLATE and reticleInstance == nil then
        reticleInstance = World.SpawnAsset(RETICLE_TEMPLATE)
    end
end

function RemoveReticle(weapon, player)
    if Object.IsValid(reticleInstance) then
        reticleInstance:Destroy()
        reticleInstance = nil
    end
    isSpawned = false
end

function OnWeaponAimChanged(player, aimingStatus)
    if not Object.IsValid(WEAPON) then return end

    if player == WEAPON.owner then
        isAiming = aimingStatus
    end
end

-- Initialize
WEAPON.unequippedEvent:Connect(RemoveReticle)
Events.Connect("WeaponAiming", OnWeaponAimChanged)