Assets {
  Id: 14772071671358103229
  Name: "Gatling Gun Muzzle Flash"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 13855099616501888772
      Objects {
        Id: 13855099616501888772
        Name: "Gatling Gun Muzzle Flash"
        Transform {
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        ChildIds: 14060675695792647354
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Folder {
          IsGroup: true
        }
      }
      Objects {
        Id: 14060675695792647354
        Name: "Client Context"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 13855099616501888772
        ChildIds: 14424748237414065504
        ChildIds: 12802724391819738132
        ChildIds: 2395597202738866681
        ChildIds: 12696080795552911832
        ChildIds: 17657597702576782503
        ChildIds: 4402160616284383659
        ChildIds: 7382084175190652332
        ChildIds: 9264626616793038618
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        NetworkContext {
        }
      }
      Objects {
        Id: 14424748237414065504
        Name: "Gunshot Assault Rifle Carbine Set 01 SFX"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 14060675695792647354
        UnregisteredParameters {
          Overrides {
            Name: "bp:Type"
            Enum {
              Value: "mc:esfx_gunshot_assaultrifle_carbine:4"
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
            Id: 3324582285615672503
          }
          TeamSettings {
          }
          AudioBP {
            AutoPlay: true
            Volume: 0.8
            Falloff: 3600
            Radius: 400
            IsSpatializationEnabled: true
            IsAttenuationEnabled: true
          }
        }
      }
      Objects {
        Id: 12802724391819738132
        Name: "Gunshot Assault Rifle Carbine Set 01 SFX"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 14060675695792647354
        UnregisteredParameters {
          Overrides {
            Name: "bp:Type"
            Enum {
              Value: "mc:esfx_gunshot_assaultrifle_carbine:6"
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
            Id: 3324582285615672503
          }
          TeamSettings {
          }
          AudioBP {
            AutoPlay: true
            Volume: 0.5
            Falloff: 3600
            Radius: 400
            IsSpatializationEnabled: true
            IsAttenuationEnabled: true
          }
        }
      }
      Objects {
        Id: 2395597202738866681
        Name: "Gunshot Assault Rifle Carbine Set 01 SFX"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 14060675695792647354
        UnregisteredParameters {
          Overrides {
            Name: "bp:Type"
            Enum {
              Value: "mc:esfx_gunshot_assaultrifle_carbine:0"
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
            Id: 3324582285615672503
          }
          TeamSettings {
          }
          AudioBP {
            AutoPlay: true
            Volume: 1
            Falloff: 3600
            Radius: 400
            IsSpatializationEnabled: true
            IsAttenuationEnabled: true
          }
        }
      }
      Objects {
        Id: 12696080795552911832
        Name: "Gunshot Pistol & Revolver Set 01 SFX"
        Transform {
          Location {
            Z: -2.28881836e-05
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 14060675695792647354
        UnregisteredParameters {
          Overrides {
            Name: "bp:Gunshot Volume"
            Float: 20
          }
          Overrides {
            Name: "bp:Ricochet Type"
            Enum {
              Value: "mc:esfx_gunshot_ricochets:2"
            }
          }
          Overrides {
            Name: "bp:Gunshot Type"
            Enum {
              Value: "mc:esfx_gunshot_pistol_revolver:4"
            }
          }
          Overrides {
            Name: "bp:Ricochet Volume"
            Float: 25
          }
          Overrides {
            Name: "bp:Gunshot Pitch"
            Float: 678.883789
          }
          Overrides {
            Name: "bp:Ricochet Pitch"
            Float: 1421.4375
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
            Id: 11671637230280120648
          }
          TeamSettings {
          }
          AudioBP {
            AutoPlay: true
            Volume: 1
            Falloff: 3600
            Radius: 400
            IsSpatializationEnabled: true
            IsAttenuationEnabled: true
          }
        }
      }
      Objects {
        Id: 17657597702576782503
        Name: "Gunshot Minigun Firing Deactivate 01 SFX"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 14060675695792647354
        Collidable_v2 {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        AudioInstance {
          AudioAsset {
            Id: 1919906127854828553
          }
          AutoPlay: true
          Volume: 1
          Falloff: 3600
          Radius: 400
          IsSpatializationEnabled: true
          IsAttenuationEnabled: true
        }
      }
      Objects {
        Id: 4402160616284383659
        Name: "Flare Billboard"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 14060675695792647354
        UnregisteredParameters {
          Overrides {
            Name: "bp:Color B"
            Color {
              R: 1
              G: 0.483
              B: 0.0603039861
              A: 1
            }
          }
          Overrides {
            Name: "bp:Color C"
            Color {
              R: 0.88
              G: 0.0174834337
              A: 1
            }
          }
          Overrides {
            Name: "bp:Depth Fade Distance"
            Float: 0
          }
          Overrides {
            Name: "bp:Camera Fade Distance"
            Float: 0
          }
          Overrides {
            Name: "bp:Camera Fade Offset"
            Float: 0
          }
          Overrides {
            Name: "bp:Flare Shape"
            Int: 3
          }
          Overrides {
            Name: "bp:Width"
            Float: 1
          }
          Overrides {
            Name: "bp:Height"
            Float: 1
          }
          Overrides {
            Name: "bp:Emissive Boost"
            Float: 15
          }
          Overrides {
            Name: "bp:Rotate Texture"
            Bool: false
          }
          Overrides {
            Name: "bp:Auto Rotate"
            Bool: true
          }
          Overrides {
            Name: "bp:Rotation Rate"
            Float: 1000
          }
        }
        Lifespan: 0.1
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
            Id: 2716613438053915724
          }
          TeamSettings {
          }
        }
      }
      Objects {
        Id: 7382084175190652332
        Name: "Rocket Launcher Muzzleflash VFX"
        Transform {
          Location {
          }
          Rotation {
            Pitch: -90
          }
          Scale {
            X: 0.1
            Y: 0.1
            Z: 0.1
          }
        }
        ParentId: 14060675695792647354
        UnregisteredParameters {
          Overrides {
            Name: "bp:Enable Sparks"
            Bool: false
          }
          Overrides {
            Name: "bp:Enable Smoke"
            Bool: true
          }
          Overrides {
            Name: "bp:Smoke Color"
            Color {
            }
          }
          Overrides {
            Name: "bp:Wind Speed"
            Vector {
            }
          }
          Overrides {
            Name: "bp:Density"
            Float: 0.01
          }
          Overrides {
            Name: "bp:Local Space"
            Bool: false
          }
          Overrides {
            Name: "bp:Particle Size Multiplier"
            Float: 1
          }
          Overrides {
            Name: "bp:Muzzle Flash Size"
            Vector {
              X: 5
              Y: 4
              Z: 4
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
            Id: 807510826300417173
          }
          TeamSettings {
          }
          Vfx {
            AutoPlay: true
          }
        }
        Relevance {
          Value: "mc:eproxyrelevance:critical"
        }
      }
      Objects {
        Id: 9264626616793038618
        Name: "Smoke Puff VFX"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 0.2
            Y: 0.2
            Z: 0.2
          }
        }
        ParentId: 14060675695792647354
        UnregisteredParameters {
          Overrides {
            Name: "bp:Density"
            Float: 0.01
          }
          Overrides {
            Name: "bp:Particle Scale Multiplier"
            Float: 3.14014602
          }
          Overrides {
            Name: "bp:Gravity"
            Float: 1
          }
          Overrides {
            Name: "bp:Wind Speed"
            Vector {
              Z: 10
            }
          }
          Overrides {
            Name: "bp:Burst"
            Bool: true
          }
          Overrides {
            Name: "bp:Radius"
            Float: 0.1
          }
          Overrides {
            Name: "bp:Life"
            Float: 2
          }
          Overrides {
            Name: "bp:color"
            Color {
              R: 1
              G: 0.961524248
              B: 0.875
              A: 0.7
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
            Id: 17772147750865925804
          }
          TeamSettings {
          }
          Vfx {
            AutoPlay: true
          }
        }
        Relevance {
          Value: "mc:eproxyrelevance:critical"
        }
      }
    }
    Assets {
      Id: 3324582285615672503
      Name: "Gunshot Assault Rifle Carbine Set 01 SFX"
      PlatformAssetType: 10
      PrimaryAsset {
        AssetType: "AudioBlueprintAssetRef"
        AssetId: "sfxabp_gunshot_assaultrifle_carbine_ref"
      }
    }
    Assets {
      Id: 11671637230280120648
      Name: "Gunshot Pistol & Revolver Set 01 SFX"
      PlatformAssetType: 10
      PrimaryAsset {
        AssetType: "AudioBlueprintAssetRef"
        AssetId: "sfxabp_gunshot_revolver_ref"
      }
    }
    Assets {
      Id: 1919906127854828553
      Name: "Gunshot Minigun Firing Deactivate 01 SFX"
      PlatformAssetType: 7
      PrimaryAsset {
        AssetType: "AudioAssetRef"
        AssetId: "sfx_minigun_firing_deactivate_01_Cue_ref"
      }
    }
    Assets {
      Id: 2716613438053915724
      Name: "Flare Billboard"
      PlatformAssetType: 6
      PrimaryAsset {
        AssetType: "BlueprintAssetRef"
        AssetId: "fxbp_flare_object"
      }
    }
    Assets {
      Id: 807510826300417173
      Name: "Rocket Launcher Muzzleflash VFX"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_rocket_launcher_muzzleflash"
      }
    }
    Assets {
      Id: 17772147750865925804
      Name: "Smoke Puff VFX"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_smoke_puff"
      }
    }
    PrimaryAssetId {
      AssetType: "None"
      AssetId: "None"
    }
  }
  SerializationVersion: 78
}
