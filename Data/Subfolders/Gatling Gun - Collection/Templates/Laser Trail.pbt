Assets {
  Id: 4654131494541027984
  Name: "Laser Trail"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 15674677536265277958
      Objects {
        Id: 15674677536265277958
        Name: "Laser Trail"
        Transform {
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        ChildIds: 8677775456725311012
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
        Id: 8677775456725311012
        Name: "Sci-fi Trails"
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
        ParentId: 15674677536265277958
        UnregisteredParameters {
          Overrides {
            Name: "bp:color"
            Color {
              R: 1
              G: 0.650000036
              A: 1
            }
          }
          Overrides {
            Name: "bp:ColorB"
            Color {
              R: 1
              G: 0.650000036
              A: 1
            }
          }
          Overrides {
            Name: "bp:ColorC"
            Color {
              R: 1
              G: 0.650000036
              A: 1
            }
          }
          Overrides {
            Name: "bp:Shape Index"
            Int: 5
          }
          Overrides {
            Name: "bp:Mask Index"
            Int: 1
          }
          Overrides {
            Name: "bp:Texture Pan Speed"
            Float: 2
          }
          Overrides {
            Name: "bp:Particle Scale Multiplier"
            Float: 2
          }
          Overrides {
            Name: "bp:Life"
            Float: 0.2
          }
          Overrides {
            Name: "bp:Hot Spot Scale"
            Float: 0
          }
          Overrides {
            Name: "bp:Emissive Boost"
            Float: 5
          }
          Overrides {
            Name: "bp:Local Space"
            Bool: false
          }
          Overrides {
            Name: "bp:Color Offset B"
            Float: 0.5
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
            Id: 12504486319384224268
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
      Id: 12504486319384224268
      Name: "Sci-fi Trails"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_trail_variants"
      }
    }
    PrimaryAssetId {
      AssetType: "None"
      AssetId: "None"
    }
  }
  SerializationVersion: 78
}
