Assets {
  Id: 17409900980798876148
  Name: "Laser Low Ammo Sound"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 15806833969915799604
      Objects {
        Id: 15806833969915799604
        Name: "Laser Low Ammo Sound"
        Transform {
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        ChildIds: 8214966745187212600
        UnregisteredParameters {
        }
        Lifespan: 2
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
        Id: 8214966745187212600
        Name: "Sci-fi UI Alert Alien Error Denied 01 SFX"
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
        ParentId: 15806833969915799604
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
            Id: 16821965836795041010
          }
          AutoPlay: true
          Volume: 0.8
          Falloff: 3600
          Radius: 400
        }
      }
    }
    Assets {
      Id: 16821965836795041010
      Name: "Sci-fi UI Alert Alien Error Denied 01 SFX"
      PlatformAssetType: 7
      PrimaryAsset {
        AssetType: "AudioAssetRef"
        AssetId: "sfx_scifi_ui_alert_alien_error_denied_01_Cue_ref"
      }
    }
    PrimaryAssetId {
      AssetType: "None"
      AssetId: "None"
    }
  }
  SerializationVersion: 78
}
