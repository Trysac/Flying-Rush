Assets {
  Id: 3405965635838317971
  Name: "Energy Glow 1"
  PlatformAssetType: 13
  SerializationVersion: 78
  CustomMaterialAsset {
    BaseMaterialId: 3911298908335124553
    ParameterOverrides {
      Overrides {
        Name: "glow color"
        Color {
          R: 1
          G: 0.650000036
          A: 1
        }
      }
      Overrides {
        Name: "roughness"
        Float: 0.4
      }
      Overrides {
        Name: "specular rolloff"
        Float: 0.5
      }
      Overrides {
        Name: "metallic"
        Float: 0
      }
      Overrides {
        Name: "glow intensity"
        Float: 5
      }
      Overrides {
        Name: "texture scale"
        Float: 1
      }
      Overrides {
        Name: "fresnelexp"
        Float: 4.39783096
      }
      Overrides {
        Name: "noise amount"
        Float: 1
      }
      Overrides {
        Name: "pulse speed"
        Float: 0.75
      }
      Overrides {
        Name: "pulse amount"
        Float: 1
      }
    }
    Assets {
      Id: 3911298908335124553
      Name: "Energy Tube Glow"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "Energy_Tube_Glow"
      }
    }
  }
}
