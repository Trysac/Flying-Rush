Assets {
  Id: 1474878663216061127
  Name: "Antigrav Muzzleflash"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 14704327219004508181
      Objects {
        Id: 14704327219004508181
        Name: "Gravty Gun Muzzleflash"
        Transform {
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        ChildIds: 633011436668939229
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
        Id: 633011436668939229
        Name: "Gravity Gun Muzzleflash"
        Transform {
          Location {
            X: 7.2479248e-05
            Y: -6.10351563e-05
          }
          Rotation {
            Pitch: -90
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 14704327219004508181
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
        Blueprint {
          BlueprintAsset {
            Id: 5073889532188189909
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
      Id: 5073889532188189909
      Name: "Plasma Muzzleflash VFX"
      PlatformAssetType: 8
      PrimaryAsset {
        AssetType: "VfxBlueprintAssetRef"
        AssetId: "fxbp_plasma_muzzleflash"
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
