Assets {
  Id: 15090055299887107812
  Name: "Custom Plasma"
  PlatformAssetType: 13
  SerializationVersion: 78
  CustomMaterialAsset {
    BaseMaterialId: 10168666001384067758
    ParameterOverrides {
      Overrides {
        Name: "outer color"
        Color {
          A: 1
        }
      }
      Overrides {
        Name: "plasma scale"
        Float: 22.883215
      }
      Overrides {
        Name: "fresnel exponent"
        Float: 7.95594
      }
      Overrides {
        Name: "threshold max"
        Float: 0.01
      }
      Overrides {
        Name: "speed"
        Vector {
          X: 5
          Y: 5
          Z: -15
        }
      }
    }
    Assets {
      Id: 10168666001384067758
      Name: "Plasma"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "fxma_plasmafield"
      }
    }
  }
}
