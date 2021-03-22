Assets {
  Id: 14926041369663842522
  Name: "Antigrav field"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 16647583239311088824
      Objects {
        Id: 16647583239311088824
        Name: "Antigrav field"
        Transform {
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        ChildIds: 12817062237768229322
        ChildIds: 16105747285999640128
        ChildIds: 16356048874927598952
        ChildIds: 5370582183361083845
        ChildIds: 15469912562422172113
        ChildIds: 12711191103630144064
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
        Id: 12817062237768229322
        Name: "Trigger"
        Transform {
          Location {
            X: -809.491943
            Y: -320.375916
            Z: -50
          }
          Rotation {
          }
          Scale {
            X: 20
            Y: 20
            Z: 60
          }
        }
        ParentId: 16647583239311088824
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
        Trigger {
          TeamSettings {
            IsTeamCollisionEnabled: true
            IsEnemyCollisionEnabled: true
          }
          TriggerShape_v2 {
            Value: "mc:etriggershape:capsule"
          }
        }
      }
      Objects {
        Id: 16105747285999640128
        Name: "WaterPhysicsVolume"
        Transform {
          Location {
            Z: 1740
          }
          Rotation {
          }
          Scale {
            X: 20
            Y: 20
            Z: 60
          }
        }
        ParentId: 16647583239311088824
        UnregisteredParameters {
          Overrides {
            Name: "bp:Volume Type"
            Enum {
              Value: "mc:evolumetype:2"
            }
          }
          Overrides {
            Name: "bp:Water"
            Bool: false
          }
          Overrides {
            Name: "bp:Caustics"
            Bool: false
          }
          Overrides {
            Name: "bp:Control Fog"
            Bool: false
          }
          Overrides {
            Name: "bp:Enabled"
            Bool: true
          }
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
        Blueprint {
          BlueprintAsset {
            Id: 2412578778936158996
          }
        }
      }
      Objects {
        Id: 16356048874927598952
        Name: "Fade"
        Transform {
          Location {
            Z: -10
          }
          Rotation {
          }
          Scale {
            Z: 20
          }
        }
        ParentId: 16647583239311088824
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 16532379345586599669
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 1136041648707525064
          }
          Teams {
            IsTeamCollisionEnabled: true
            IsEnemyCollisionEnabled: true
          }
          InteractWithTriggers: true
          StaticMesh {
            Physics {
            }
          }
        }
      }
      Objects {
        Id: 5370582183361083845
        Name: "Plasma"
        Transform {
          Location {
            Z: -10
          }
          Rotation {
          }
          Scale {
            Z: 40
          }
        }
        ParentId: 16647583239311088824
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15090055299887107812
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 1136041648707525064
          }
          Teams {
            IsTeamCollisionEnabled: true
            IsEnemyCollisionEnabled: true
          }
          InteractWithTriggers: true
          StaticMesh {
            Physics {
            }
          }
        }
      }
      Objects {
        Id: 15469912562422172113
        Name: "Dust Motes Volume VFX"
        Transform {
          Location {
            Z: 660
          }
          Rotation {
          }
          Scale {
            X: 15
            Y: 15
            Z: 13.5000048
          }
        }
        ParentId: 16647583239311088824
        UnregisteredParameters {
          Overrides {
            Name: "bp:color"
            Color {
              R: 0.0843708888
              B: 0.909999967
              A: 1
            }
          }
          Overrides {
            Name: "bp:Curl Variance"
            Float: 0
          }
          Overrides {
            Name: "bp:Curl Speed"
            Vector {
            }
          }
          Overrides {
            Name: "bp:Curl Offset"
            Vector {
            }
          }
          Overrides {
            Name: "bp:Wind Speed"
            Vector {
            }
          }
          Overrides {
            Name: "bp:Emissive Boost"
            Float: 50
          }
          Overrides {
            Name: "bp:Gravity"
            Float: 1
          }
          Overrides {
            Name: "bp:Particle Scale Multiplier"
            Float: 0.99455905
          }
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
        Blueprint {
          BlueprintAsset {
            Id: 1605147525139287286
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
        Id: 12711191103630144064
        Name: "GravityExplosion"
        Transform {
          Location {
            Z: 99999
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 16647583239311088824
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
        Script {
          ScriptAsset {
            Id: 12348327139879758326
          }
        }
      }
    }
    Assets {
      Id: 2412578778936158996
      Name: "Underwater Post Process"
      PlatformAssetType: 6
      PrimaryAsset {
        AssetType: "BlueprintAssetRef"
        AssetId: "fxbp_post_process_water"
      }
    }
    Assets {
      Id: 1136041648707525064
      Name: "Cylinder - Bottom-Aligned"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_cylinder_001"
      }
    }
    Assets {
      Id: 16532379345586599669
      Name: "Additive Edgeline Top Fade"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "fxmi_add_edgeline_sharpline_topfade"
      }
    }
    Assets {
      Id: 1605147525139287286
      Name: "Dust Motes Volume VFX"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_dust_motes"
      }
    }
    PrimaryAssetId {
      AssetType: "None"
      AssetId: "None"
    }
  }
  Marketplace {
    Description: "Gun that createss antigravity fields.\r\n\r\nCreated for CORE daily challenge #1!\r\n\r\nNote:  You\'ll need to set your player movement setteings to have some water buoyancy for this to work, since that\'s how the antigravity effect happens.  I recommend values of around 2."
  }
  SerializationVersion: 78
  DirectlyPublished: true
}
