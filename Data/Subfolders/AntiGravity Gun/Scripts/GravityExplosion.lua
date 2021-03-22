local top = script.parent
local water = top:FindChildByName("WaterPhysicsVolume")
local trigger = top:FindChildByName("Trigger")

water:SetPosition(Vector3.New(0, 0, 0))

local stuffHit = trigger:GetOverlappingObjects()

for k, v in pairs(stuffHit) do
	if v:IsA("Player") then
		v:AddImpulse(Vector3.New(0, 0, 1))
	end
end



top:SetWorldRotation(Rotation.New(0, 0, 0))

effects = {
	top:FindChildByName("Fade"),
	top:FindChildByName("Plasma"),
}


for k, v in pairs(effects) do
	v:ScaleTo(Vector3.New(20, 20, v:GetWorldScale().z), 0.5)
end

Task.Wait(3)

--water:Destroy()

for k, v in pairs(effects) do
	v:ScaleTo(Vector3.New(0, 0, v:GetWorldScale().z), 0.5)
end

Task.Wait(1)

top:Destroy()