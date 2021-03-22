local gun = script.parent
local propAntigravField = script:GetCustomProperty("AntigravField")


function OnHitSomething(weapon, interaction)
	local hr = interaction:GetHitResult()
	World.SpawnAsset(propAntigravField, {position = hr:GetImpactPosition() + Vector3.New(0, 0, -500)})
end

gun.targetImpactedEvent:Connect(OnHitSomething)

