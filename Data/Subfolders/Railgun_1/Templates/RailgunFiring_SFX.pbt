Assets {
  Id: 5196707298574325179
  Name: "RailgunFiring_SFX"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 2689592157740228678
      Objects {
        Id: 2689592157740228678
        Name: "RailgunFiring"
        Transform {
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        UnregisteredParameters {
          Overrides {
            Name: "bp:Type"
            Enum {
              Value: "mc:esfx_retro_arcade_laser_shots_01:14"
            }
          }
        }
        Collidable_v2 {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Blueprint {
          BlueprintAsset {
            Id: 2379605741215362764
          }
          AudioBP {
            Pitch: -2286.61426
            Volume: 1.35433066
            Falloff: 3600
            Radius: 400
            EnableOcclusion: true
            IsSpatializationEnabled: true
            IsAttenuationEnabled: true
          }
        }
      }
    }
    Assets {
      Id: 2379605741215362764
      Name: "Gunshot Retro & Arcade Laser Shots Set 01 SFX"
      PlatformAssetType: 10
      PrimaryAsset {
        AssetType: "AudioBlueprintAssetRef"
        AssetId: "sfxabp_gunshot_retro_arcade_laser_shots_01_ref"
      }
    }
    PrimaryAssetId {
      AssetType: "None"
      AssetId: "None"
    }
  }
  SerializationVersion: 78
}
