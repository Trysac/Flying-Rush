local speed = 0

function OnPlayerRespawn(player)
	player.animationStance = "unarmed_stance"
	Task.Wait(0.05)
	player.animationStance = "2hand_rifle_aim_shoulder"
	player.maxWalkSpeed = speed
end

function OnPlayerJoined(player)
	player.respawnedEvent:Connect(OnPlayerRespawn)
	if speed == 0 then
		speed = player.maxWalkSpeed
	end
end 

Game.playerJoinedEvent:Connect(OnPlayerJoined)