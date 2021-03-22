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

--[[
	lol y u here
	look that's a license ^
	


	Standard FPS aiming-down-sight-like script, built on top of the default WeaponAimClient script.
	Spent too much time and brainpower on this.
	Seriously. I had to use actual m a t h.
	Not fun. Trust me.
	Also- just imagine me sitting here like 10 hours just squashing every single bug so you can happily
	use this basic aiming system. That definitely doesn't sound fun.
	And I bet there's still a ton of them left...
	
	...speaking of...

	...DM me on discord if you find any.
															- chipnertkj#5685
--]]


-- Internal custom properties
local WEAPON = script:FindAncestorByType('Weapon')
if not WEAPON:IsA('Weapon') then
    error(script.name .. " should be part of Weapon object hierarchy.")
end

-- Exposed variables --
local CAN_AIM = true
local AIM_BINDING = WEAPON:GetCustomProperty("AimBinding")
local FOV = WEAPON:GetCustomProperty("AimFOV")

-- Internal handle variables --
local pressedHandle = nil              -- Event handle when player presses the aim binding
local releasedHandle = nil             -- Event handle when player releases the aim binding
local playerDieHandle = nil            -- Event handle when player dies

-- Internal camera variables --
local defaultCamera = nil
local p = Game.GetLocalPlayer()

local art = WEAPON:FindDescendantByName("Geo")
local isAiming = false
local scopeCamera = World.FindObjectByName("Scope Camera")
local rot = p:GetViewWorldRotation()
local off = scopeCamera:GetPositionOffset()
local yy = Vector3.New(0)

function Approach(a, b, c)
    if a < b then
        a = a + c
    end
    if a > b then
        a = a - c
    end
    return a
end

function Tick(deltaTime)
    if not CAN_AIM then return end
    if not Object.IsValid(WEAPON) then return end
    if WEAPON.owner ~= p then return end

    -- We call OnEquipped function after player is fully loaded in client
    if Object.IsValid(WEAPON.owner) and not Object.IsValid(defaultCamera) then
    		OnEquipped(WEAPON, WEAPON.owner)
	end
	if isAiming then
		scopeCamera.fieldOfView = FOV
		p:SetOverrideCamera(scopeCamera, 0.15)
	else
        p:ClearOverrideCamera(0.15)
	end

    -- Smoothly lerps the camera distance when player aims
    LerpCameraDistance(deltaTime)
end

 -- Moves the camera into position over time
function LerpCameraDistance(deltaTime)
	if not Object.IsValid(defaultCamera) then return end
	if not isAiming then p:ClearOverrideCamera(0.05) return end
	rot = p:GetViewWorldRotation()
	scopeCamera:SetWorldRotation(Rotation.New(Quaternion.Slerp(
		Quaternion.New(scopeCamera:GetWorldRotation()),
		Quaternion.New(art:GetWorldRotation()),
		deltaTime*50))
	)
	local v = -p:GetWorldRotation() * p:GetVelocity()
	yy.y = Approach(yy.y, v.y/62.5, math.abs(yy.y - v.y/62.5)/3)
    if v.size < 0.15 then
		scopeCamera:SetWorldPosition((scopeCamera:GetWorldPosition() + rot*yy + art:GetWorldPosition())/2)
	else
		scopeCamera:SetWorldPosition(art:GetWorldPosition() + rot*yy)
	end
end

 -- Gets player current active camera
function GetPlayerdefaultCamera(player)
    if not Object.IsValid(player) then
        return nil
    end
	local fpc = World.FindObjectByName("FP Camera") 
	if fpc == nil then
		error("Please use the FPS ADS Camera Settings available in the community content.")
	end
	return fpc
end

function EnableScoping(player)
    if player.isDead then return end
    lerpTime = 0
    isAiming = true
	Events.Broadcast("WeaponAiming", player, true)
end

function ResetScoping(player)
    lerpTime = 0
    isAiming  = false
    Events.Broadcast("WeaponAiming", player, false)
end

function OnBindingPressed(player, actionName)
    if actionName == AIM_BINDING then
        EnableScoping(player)
	end
end

function OnBindingReleased(player, actionName)
    if actionName == AIM_BINDING then
        ResetScoping(player)
	end
end

function OnPlayerDied(player, damage)
	ResetScoping(player)
	WEAPON:Unequip()
end

function OnEquipped(weapon, player)
    if not CAN_AIM then return end
    if not Object.IsValid(player) then return end

    -- Register event handles
    pressedHandle = player.bindingPressedEvent:Connect(OnBindingPressed)
    releasedHandle = player.bindingReleasedEvent:Connect(OnBindingReleased)
    playerDieHandle = player.diedEvent:Connect(OnPlayerDied)

    -- Set new active camera
    defaultCamera = GetPlayerdefaultCamera(player)
	lerpTime = 0
	isAiming = false
end

function OnUnequipped(weapon, player)
    if not CAN_AIM then return end

    -- Disconnects all the handle events to avoid event trigger
    -- for previous player when the weapon is used by next player
	if pressedHandle then pressedHandle:Disconnect() end
	if releasedHandle then releasedHandle:Disconnect() end
    if playerDieHandle then playerDieHandle:Disconnect() end

    ResetScoping(player)

    -- Remove the reference to the camera
    if Object.IsValid(defaultCamera) then
        defaultCamera = nil
    end
end

-- Initialize
WEAPON.unequippedEvent:Connect(OnUnequipped)