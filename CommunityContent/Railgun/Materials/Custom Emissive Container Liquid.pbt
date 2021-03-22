Assets {
  Id: 4690086130386063043
  Name: "Custom Emissive Container Liquid"
  PlatformAssetType: 13
  SerializationVersion: 78
  CustomMaterialAsset {
    BaseMaterialId: 17430582278164592873
    ParameterOverrides {
      Overrides {
        Name: "surface height"
        Float: 1
      }
      Overrides {
        Name: "color"
        Color {
          R: 0.750000179
          G: 9.62085533
          B: 15
          A: 1
        }
      }
      Overrides {
        Name: "inner color"
        Color {
          G: 0.171523124
          B: 0.700000048
          A: 1
        }
      }
      Overrides {
        Name: "top color"
        Color {
          R: 0.36
          G: 0.605827451
          B: 1
          A: 1
        }
      }
    }
    Assets {
      Id: 17430582278164592873
      Name: "Emissive Container Liquid"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "fxmi_emissive_cutoff_liquid"
      }
    }
  }
}
