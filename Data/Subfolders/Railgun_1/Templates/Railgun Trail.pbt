Assets {
  Id: 2834710456922177451
  Name: "Railgun Trail"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 5445494803744645023
      Objects {
        Id: 5445494803744645023
        Name: "Railgun Trail"
        Transform {
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        ChildIds: 4153250350387333018
        ChildIds: 793208066629883453
        ChildIds: 1742336062534961517
        ChildIds: 4351531772773339920
        UnregisteredParameters {
        }
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
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
        Id: 4153250350387333018
        Name: "RailgunFiring_SFX"
        Transform {
          Location {
            Z: -1.52587891e-05
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 5445494803744645023
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
      Objects {
        Id: 793208066629883453
        Name: "Magic Projectile VFX"
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
        ParentId: 5445494803744645023
        UnregisteredParameters {
          Overrides {
            Name: "bp:color"
            Color {
              R: 0.0600000024
              G: 0.682516396
              B: 1
              A: 1
            }
          }
          Overrides {
            Name: "bp:Emissive Boost"
            Float: 14.4094486
          }
          Overrides {
            Name: "bp:Secondary Color"
            Color {
              G: 0.289271414
              B: 0.480000019
              A: 1
            }
          }
          Overrides {
            Name: "bp:Secondary Emissive Boost"
            Float: 18.661417
          }
          Overrides {
            Name: "bp:Density"
            Float: 3.85795283
          }
          Overrides {
            Name: "bp:Particle Scale Multiplier"
            Float: 2.39181089
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
            Id: 15917324277267578461
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
        Id: 1742336062534961517
        Name: "Railgun Projectile"
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
        ParentId: 5445494803744645023
        UnregisteredParameters {
          Overrides {
            Name: "bp:ColorB"
            Color {
              R: 0.13
              G: 0.533311
              B: 1
              A: 1
            }
          }
          Overrides {
            Name: "bp:ColorC"
            Color {
              G: 0.241589367
              B: 0.480000019
              A: 1
            }
          }
          Overrides {
            Name: "bp:Emissive Boost"
            Float: 25.1181087
          }
          Overrides {
            Name: "bp:Life"
            Float: 0.5
          }
          Overrides {
            Name: "bp:color"
            Color {
              R: 0.45
              G: 0.83609271
              B: 1
              A: 1
            }
          }
          Overrides {
            Name: "bp:Particle Scale Multiplier"
            Float: 1.15
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
            Id: 17977280587505271142
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
        Id: 4351531772773339920
        Name: "Distortion Ring Trail VFX"
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
        ParentId: 5445494803744645023
        UnregisteredParameters {
          Overrides {
            Name: "bp:Density"
            Float: 1.66645682
          }
          Overrides {
            Name: "bp:Particle Scale Multiplier"
            Float: 0.770393729
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
            Id: 15197103679429869035
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
      Id: 2379605741215362764
      Name: "Gunshot Retro & Arcade Laser Shots Set 01 SFX"
      PlatformAssetType: 10
      PrimaryAsset {
        AssetType: "AudioBlueprintAssetRef"
        AssetId: "sfxabp_gunshot_retro_arcade_laser_shots_01_ref"
      }
    }
    Assets {
      Id: 15917324277267578461
      Name: "Magic Projectile VFX"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_magic_projectile"
      }
    }
    Assets {
      Id: 17977280587505271142
      Name: "Basic Projectile Trail VFX"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_basic_projectile_trail"
      }
    }
    Assets {
      Id: 15197103679429869035
      Name: "Distortion Ring Trail VFX"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_distortion_trail"
      }
    }
    PrimaryAssetId {
      AssetType: "None"
      AssetId: "None"
    }
  }
  SerializationVersion: 78
}
