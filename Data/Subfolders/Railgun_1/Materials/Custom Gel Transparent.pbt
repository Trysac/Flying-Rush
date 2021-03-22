Assets {
  Id: 17047147770589865149
  Name: "Custom Gel Transparent"
  PlatformAssetType: 13
  SerializationVersion: 78
  CustomMaterialAsset {
    BaseMaterialId: 355084766817047955
    ParameterOverrides {
      Overrides {
        Name: "color"
        Color {
          R: 0.24000001
          G: 0.728211522
          B: 1
          A: 1
        }
      }
      Overrides {
        Name: "inner color"
        Color {
          G: 0.31788075
          B: 0.75
          A: 1
        }
      }
    }
    Assets {
      Id: 355084766817047955
      Name: "Gel Transparent"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "fxmi_gel"
      }
    }
  }
}
