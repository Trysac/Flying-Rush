Assets {
  Id: 8133779887752947350
  Name: "Railgun"
  PlatformAssetType: 5
  TemplateAsset {
    ObjectBlock {
      RootId: 1848594543195918722
      Objects {
        Id: 1848594543195918722
        Name: "Railgun"
        Transform {
          Scale {
            X: 0.99999994
            Y: 0.99999994
            Z: 1
          }
        }
        ParentId: 4781671109827199097
        ChildIds: 4484691240407917499
        ChildIds: 11841752165988337079
        ChildIds: 972788928337400750
        ChildIds: 3865867001883727515
        ChildIds: 6708322448160599002
        ChildIds: 5696031600611268807
        UnregisteredParameters {
          Overrides {
            Name: "cs:BaseDamage"
            Float: 45
          }
          Overrides {
            Name: "cs:HeadshotDamage"
            Float: 65
          }
          Overrides {
            Name: "cs:EnableAutoReload"
            Bool: true
          }
          Overrides {
            Name: "cs:EnableAim"
            Bool: true
          }
          Overrides {
            Name: "cs:AimBinding"
            String: "ability_secondary"
          }
          Overrides {
            Name: "cs:AimActiveStance"
            String: "2hand_rifle_aim_shoulder"
          }
          Overrides {
            Name: "cs:AimWalkSpeedPercentage"
            Float: 0.5
          }
          Overrides {
            Name: "cs:AimZoomDistance"
            Float: 100
          }
          Overrides {
            Name: "cs:SpreadStandPrecision"
            Float: 0.65
          }
          Overrides {
            Name: "cs:SpreadWalkPrecision"
            Float: 0.35
          }
          Overrides {
            Name: "cs:SpreadJumpPrecision"
            Float: 0.1
          }
          Overrides {
            Name: "cs:SpreadCrouchPrecision"
            Float: 0.75
          }
          Overrides {
            Name: "cs:SpreadSlidingPrecision"
            Float: 0.25
          }
          Overrides {
            Name: "cs:SpreadAimModifierBonus"
            Float: 0.3
          }
          Overrides {
            Name: "cs:PickupSound"
            AssetReference {
              Id: 15152629197498477940
            }
          }
          Overrides {
            Name: "cs:LowAmmoSound"
            AssetReference {
              Id: 16706291286472155546
            }
          }
          Overrides {
            Name: "cs:ReticleTemplate"
            AssetReference {
              Id: 18438583034954324610
            }
          }
          Overrides {
            Name: "cs:HideReticleOnAim"
            Bool: false
          }
          Overrides {
            Name: "cs:BaseDamage:tooltip"
            String: "Normal damage that this weapon will do to enemy players."
          }
          Overrides {
            Name: "cs:HeadshotDamage:tooltip"
            String: "Headshot damage that this weapon will do to enemy players."
          }
          Overrides {
            Name: "cs:AimBinding:tooltip"
            String: "Keybinding to hold and activate scope / zoom ability for the weapon. Default is \"ability_secondary\" (right mouse button)."
          }
          Overrides {
            Name: "cs:AimWalkSpeedPercentage:tooltip"
            String: "Percentage walk speed reduction when player is aiming. Must be in the range of 0 to 1."
          }
          Overrides {
            Name: "cs:AimActiveStance:tooltip"
            String: "Animation stance when aiming with this weapon. Default is 2hand_rifle_aim_shoulder."
          }
          Overrides {
            Name: "cs:AimZoomDistance:tooltip"
            String: "Camera distance when aiming."
          }
          Overrides {
            Name: "cs:SpreadAimModifierBonus:tooltip"
            String: "Amount of % added to the spread precision when the player aims. Range between 0 to 1."
          }
          Overrides {
            Name: "cs:SpreadStandPrecision:tooltip"
            String: "Precision % when the player is standing without aim. Range between 0 to 1 (from least accurate to most accurate)."
          }
          Overrides {
            Name: "cs:SpreadWalkPrecision:tooltip"
            String: "Precision % when the player is walking. Range between 0 to 1 (from least accurate to most accurate)."
          }
          Overrides {
            Name: "cs:SpreadJumpPrecision:tooltip"
            String: "Precision % when the player is jumping. Range between 0 to 1 (from least accurate to most accurate)."
          }
          Overrides {
            Name: "cs:SpreadCrouchPrecision:tooltip"
            String: "Precision % when the player is crouching. Range between 0 to 1 (from least accurate to most accurate)."
          }
          Overrides {
            Name: "cs:SpreadSlidingPrecision:tooltip"
            String: "Precision % when the player is sliding. Range between 0 to 1 (from least accurate to most accurate)."
          }
          Overrides {
            Name: "cs:EnableAutoReload:tooltip"
            String: "If true the weapon the player will auto reload the ammo without player\'s input. Only reloads if the player has the ammo resource."
          }
          Overrides {
            Name: "cs:EnableAim:tooltip"
            String: "Enable aiming for the weapon."
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
        Equipment {
          SocketName: "right_prop"
          PickupTrigger {
            SubObjectId: 3865867001883727515
          }
          Weapon {
            ProjectileAssetRef {
              Id: 14996036224905541899
            }
            MuzzleFlashAssetRef {
              Id: 14460226019985806595
            }
            TrailAssetRef {
              Id: 11738745838406812138
            }
            ImpactAssetRef {
              Id: 3882825639284567060
            }
            UseReticle: true
            Muzzle {
              Location {
                X: 90
                Z: 17.5
              }
            }
            AnimationSet: "2hand_rifle_stance"
            OutOfAmmoSfxAssetRef {
              Id: 15488498464309008336
            }
            ReloadSfxAssetRef {
              Id: 16409934749136224299
            }
            ShootAnimation: "2hand_rifle_shoot"
            ImpactProjectileAssetRef {
              Id: 841534158063459245
            }
            IsHitscan: true
            BeamAssetRef {
              Id: 841534158063459245
            }
            BurstCount: 1
            BurstDuration: 2
            BurstStopsWithRelease: true
            AttackCooldown: 0.25
            Range: 10000
            ImpactPlayerAssetRef {
              Id: 3882825639284567060
            }
            ReticleType {
              Value: "mc:ereticletype:none"
            }
            MaxAmmo: 30
            AmmoType: "rounds"
            MultiShot: 1
            ProjectileSpeed: 25000
            ProjectileLifeSpan: 10
            ProjectileLength: 50
            ProjectileRadius: 5
            ProjectilePierces: 5
            DefaultAbility {
              SubObjectId: 6708322448160599002
            }
            ReloadAbility {
              SubObjectId: 5696031600611268807
            }
            Damage: 50
          }
        }
      }
      Objects {
        Id: 4484691240407917499
        Name: "Server Context"
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
        ParentId: 1848594543195918722
        ChildIds: 14884976801765734595
        ChildIds: 14037131586463824218
        ChildIds: 7889825692850143785
        UnregisteredParameters {
        }
        WantsNetworking: true
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
          Type: Server
        }
      }
      Objects {
        Id: 14884976801765734595
        Name: "WeaponDamageShootServer"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 4484691240407917499
        UnregisteredParameters {
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
        Script {
          ScriptAsset {
            Id: 17268521430242758028
          }
        }
      }
      Objects {
        Id: 14037131586463824218
        Name: "WeaponAimServer"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 4484691240407917499
        UnregisteredParameters {
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
        Script {
          ScriptAsset {
            Id: 10891743005384501651
          }
        }
      }
      Objects {
        Id: 7889825692850143785
        Name: "WeaponPickupTriggerServer"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 4484691240407917499
        UnregisteredParameters {
          Overrides {
            Name: "cs:Trigger"
            ObjectReference {
              SubObjectId: 3865867001883727515
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
        Script {
          ScriptAsset {
            Id: 16938010343464140770
          }
        }
      }
      Objects {
        Id: 11841752165988337079
        Name: "Client Context"
        Transform {
          Location {
            Z: -1.20645905
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 1848594543195918722
        ChildIds: 2515691161844709768
        ChildIds: 770298887944028999
        UnregisteredParameters {
        }
        WantsNetworking: true
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
        Id: 2515691161844709768
        Name: "Scripts"
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
        ParentId: 11841752165988337079
        ChildIds: 16221897409593104296
        ChildIds: 14129748635976896383
        ChildIds: 11796771354660313737
        ChildIds: 3218224783893971238
        ChildIds: 15104106111175163010
        UnregisteredParameters {
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
        Folder {
          IsFilePartition: true
          FilePartitionName: "Scripts_2"
        }
      }
      Objects {
        Id: 16221897409593104296
        Name: "WeaponAimClient"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 2515691161844709768
        UnregisteredParameters {
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
        Script {
          ScriptAsset {
            Id: 5748227357091499960
          }
        }
      }
      Objects {
        Id: 14129748635976896383
        Name: "WeaponSpreadClient"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 2515691161844709768
        UnregisteredParameters {
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
        Script {
          ScriptAsset {
            Id: 3687632497521964707
          }
        }
      }
      Objects {
        Id: 11796771354660313737
        Name: "WeaponReticleClient"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 2515691161844709768
        UnregisteredParameters {
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
        Script {
          ScriptAsset {
            Id: 4439228550413230328
          }
        }
      }
      Objects {
        Id: 3218224783893971238
        Name: "WeaponAmmoFeedbackClient"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 2515691161844709768
        UnregisteredParameters {
          Overrides {
            Name: "cs:AttackAbility"
            ObjectReference {
              SubObjectId: 6708322448160599002
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
        Script {
          ScriptAsset {
            Id: 14973664172911244123
          }
        }
      }
      Objects {
        Id: 15104106111175163010
        Name: "WeaponAutoReloadClient"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 2515691161844709768
        UnregisteredParameters {
          Overrides {
            Name: "cs:ReloadAbility"
            ObjectReference {
              SubObjectId: 5696031600611268807
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
        Script {
          ScriptAsset {
            Id: 16961751622927030358
          }
        }
      }
      Objects {
        Id: 770298887944028999
        Name: "Geo"
        Transform {
          Location {
          }
          Rotation {
          }
          Scale {
            X: 1.00000012
            Y: 1.00000012
            Z: 1
          }
        }
        ParentId: 11841752165988337079
        ChildIds: 13681526016636410805
        ChildIds: 14288703383843937927
        ChildIds: 7226149329638566958
        ChildIds: 12737051880493510017
        ChildIds: 2890958988030734478
        ChildIds: 10756868736832587965
        ChildIds: 5207247502379593117
        ChildIds: 17650656475783925528
        ChildIds: 8957800760188279761
        ChildIds: 7339529587475873111
        ChildIds: 16279579263763301950
        ChildIds: 966749264250367074
        UnregisteredParameters {
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
        Folder {
          IsGroup: true
        }
      }
      Objects {
        Id: 13681526016636410805
        Name: "Modern Weapon - Stock 01"
        Transform {
          Location {
            X: -2.22470856
            Y: -2.48977121e-05
            Z: 15.5089273
          }
          Rotation {
          }
          Scale {
            X: 1.13179314
            Y: 1
            Z: 1
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
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
          Value: "mc:ecollisionsetting:forceoff"
        }
        CoreMesh {
          MeshAsset {
            Id: 499697514733272876
          }
          Teams {
            IsTeamCollisionEnabled: true
            IsEnemyCollisionEnabled: true
          }
          DisableReceiveDecals: true
          InteractWithTriggers: true
          StaticMesh {
            Physics {
            }
          }
        }
      }
      Objects {
        Id: 14288703383843937927
        Name: "Modern Weapon - Grip 01"
        Transform {
          Location {
            X: 2.79868603
            Y: -2.48977121e-05
            Z: 5.86524868
          }
          Rotation {
            Pitch: -19.9999943
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
        }
        Collidable_v2 {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceoff"
        }
        CoreMesh {
          MeshAsset {
            Id: 13155471131385409602
          }
          Teams {
            IsTeamCollisionEnabled: true
            IsEnemyCollisionEnabled: true
          }
          DisableReceiveDecals: true
          InteractWithTriggers: true
          StaticMesh {
            Physics {
            }
          }
        }
      }
      Objects {
        Id: 7226149329638566958
        Name: "Trigger - Rear"
        Transform {
          Location {
            X: 9.52880859
            Y: -2.48977121e-05
            Z: 7.81673908
          }
          Rotation {
            Yaw: -90
            Roll: 10.0530453
          }
          Scale {
            X: 0.0110827358
            Y: 0.0295748301
            Z: 0.0530300215
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 132672053610873933
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:color"
            Color {
              R: 0.149
              G: 0.149
              B: 0.149
              A: 1
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
          Value: "mc:ecollisionsetting:forceoff"
        }
        CoreMesh {
          MeshAsset {
            Id: 16965777294932964901
          }
          Teams {
            IsTeamCollisionEnabled: true
            IsEnemyCollisionEnabled: true
          }
          DisableReceiveDecals: true
          InteractWithTriggers: true
          StaticMesh {
            Physics {
            }
          }
        }
      }
      Objects {
        Id: 12737051880493510017
        Name: "Modern Weapon - Body 03"
        Transform {
          Location {
            X: 22.0055237
            Z: 20
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 0.710737467
            Z: 1
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
        }
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
            Id: 5781445960335659647
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
        Id: 2890958988030734478
        Name: "Modern Weapon - Body 01"
        Transform {
          Location {
            X: -2.73936844
            Z: 12.3253174
          }
          Rotation {
            Yaw: -179.999954
          }
          Scale {
            X: 0.762407601
            Y: 0.520863116
            Z: 0.792223096
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
          Overrides {
            Name: "ma:Shared_Trim:id"
            AssetReference {
              Id: 15375941495231049454
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13077624968254610965
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
        Id: 10756868736832587965
        Name: "Modern Weapon - Foregrip 02"
        Transform {
          Location {
            X: -7.99447632
            Z: 15
          }
          Rotation {
          }
          Scale {
            X: 0.5
            Y: 0.550320745
            Z: 1
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_Trim:id"
            AssetReference {
              Id: 15375941495231049454
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 14723360564525113759
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
        Id: 5207247502379593117
        Name: "Modern Weapon - Grenade Canister 02"
        Transform {
          Location {
            X: 0.975860596
            Z: 18.7305756
          }
          Rotation {
            Pitch: -69.427536
            Yaw: 9.2685434e-12
            Roll: 2.73205424e-05
          }
          Scale {
            X: 0.590785682
            Y: 0.590785682
            Z: 0.590785682
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 4690086130386063043
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15095053435971896742
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
        Id: 17650656475783925528
        Name: "Modern Weapon - Grenade Canister 02"
        Transform {
          Location {
            X: 0.975860596
            Z: 18.7305756
          }
          Rotation {
            Pitch: 6.83018879e-06
            Yaw: 69.427536
            Roll: -89.9999619
          }
          Scale {
            X: 0.590785682
            Y: 0.590785682
            Z: 0.590785682
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 4690086130386063043
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15095053435971896742
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
        Id: 8957800760188279761
        Name: "Modern Weapon - Grenade Canister 02"
        Transform {
          Location {
            X: 0.975860596
            Z: 18.7305756
          }
          Rotation {
            Yaw: -69.4275513
            Roll: 90.0000076
          }
          Scale {
            X: 0.590785682
            Y: 0.590785682
            Z: 0.590785682
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 4690086130386063043
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15095053435971896742
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
        Id: 7339529587475873111
        Name: "Grenade Canister 01"
        Transform {
          Location {
            X: 12.7886124
            Z: 23.4862213
          }
          Rotation {
            Pitch: -90
            Yaw: -6.10351563e-05
            Roll: 0.00012158676
          }
          Scale {
            X: 0.610335767
            Y: 0.610335767
            Z: 0.610335767
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 4690086130386063043
            }
          }
          Overrides {
            Name: "ma:Shared_Detail1:id"
            AssetReference {
              Id: 15375941495231049454
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 7409974748338403079
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
        Id: 16279579263763301950
        Name: "Grenade Canister 01"
        Transform {
          Location {
            X: 12.7886124
            Y: 4.948
            Z: 18.8056793
          }
          Rotation {
            Pitch: -90
            Yaw: -5.46415104e-05
            Roll: 0.00012158676
          }
          Scale {
            X: 0.610335767
            Y: 0.610335767
            Z: 0.610335767
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 4690086130386063043
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 7409974748338403079
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
        Id: 966749264250367074
        Name: "Grenade Canister 01"
        Transform {
          Location {
            X: 12.7886124
            Y: -4.948
            Z: 18.8056793
          }
          Rotation {
            Pitch: -90
            Yaw: -0.000109283021
            Roll: 0.000169834617
          }
          Scale {
            X: 0.610335767
            Y: 0.610335767
            Z: 0.610335767
          }
        }
        ParentId: 770298887944028999
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 4690086130386063043
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
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 7409974748338403079
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
        Id: 972788928337400750
        Name: "Geo2"
        Transform {
          Location {
            X: 27.0055275
            Z: 15.2979507
          }
          Rotation {
          }
          Scale {
            X: 0.847935438
            Y: 0.847935438
            Z: 0.847935319
          }
        }
        ParentId: 1848594543195918722
        ChildIds: 10862359830323941493
        ChildIds: 3621751509336842604
        ChildIds: 14502098600703179225
        ChildIds: 11426147671452373858
        ChildIds: 13366616934318924351
        ChildIds: 6712493161654635218
        ChildIds: 13198505940496247226
        ChildIds: 17386555362833662145
        ChildIds: 10542624181003926364
        ChildIds: 5523085111066544486
        ChildIds: 8655009364240378567
        ChildIds: 12384428516559963586
        ChildIds: 6203210214415092549
        ChildIds: 3426068431987705660
        ChildIds: 12537953230251670080
        ChildIds: 9328885932881089165
        ChildIds: 10277425235323576471
        ChildIds: 3984816766769042986
        ChildIds: 9239214801683291888
        ChildIds: 882013316385660966
        ChildIds: 8813141093374022262
        ChildIds: 5457433386744341140
        ChildIds: 15638490643766118142
        ChildIds: 426172065337851332
        ChildIds: 9860088749585857623
        ChildIds: 12728615619235702336
        ChildIds: 1523549327104743604
        ChildIds: 14267265505400304807
        ChildIds: 6123106475970478214
        ChildIds: 13224037466502210629
        ChildIds: 9942268804713016823
        ChildIds: 4541501496141212901
        ChildIds: 17289058737757856298
        ChildIds: 11272703844539274545
        ChildIds: 9598404518457013145
        ChildIds: 16614677249179763360
        ChildIds: 18312192889408922643
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
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
        Id: 10862359830323941493
        Name: "Ellipsoid - Truncated Hollow Wedge"
        Transform {
          Location {
            Z: 2.42007017
          }
          Rotation {
            Pitch: -90
            Yaw: -179.36705
            Roll: 179.367188
          }
          Scale {
            X: 0.25000003
            Y: 0.235682979
            Z: 0.403979242
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 3842114781646480660
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17443924865729885564
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
        Id: 3621751509336842604
        Name: "Ellipsoid - Truncated Hollow Wedge"
        Transform {
          Location {
            Z: 2.42007017
          }
          Rotation {
            Pitch: 6.83018879e-06
            Yaw: -90
            Roll: 90.0000687
          }
          Scale {
            X: 0.2356655
            Y: 0.250000238
            Z: 0.403979152
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 3842114781646480660
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17443924865729885564
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
        Id: 14502098600703179225
        Name: "Pipe - 90-Degree Short Thick"
        Transform {
          Location {
            X: 23.7879524
            Z: -7.67088
          }
          Rotation {
            Pitch: 90
            Yaw: 25.2393742
            Roll: -154.760651
          }
          Scale {
            X: 0.0618341677
            Y: 0.0618341677
            Z: 0.0618341714
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17223073294780175050
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
        Id: 11426147671452373858
        Name: "Modern Weapon Accessory - Shell Clip"
        Transform {
          Location {
            X: -3.16224265
            Y: 4.25961399
            Z: -19.1575127
          }
          Rotation {
            Pitch: 90
          }
          Scale {
            X: 1.4741689
            Y: 1.4741689
            Z: 1.47416902
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 5227383618723814188
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:color"
            Color {
              G: 0.572715044
              B: 0.919999957
              A: 1
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6157711858907400962
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
        Id: 13366616934318924351
        Name: "Modern Weapon Barrel 01"
        Transform {
          Location {
            X: -11.568078
            Z: 1.3907882
          }
          Rotation {
          }
          Scale {
            X: 2.2830236
            Y: 1.17933512
            Z: 1.17933524
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 2817890560817570586
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
        Id: 6712493161654635218
        Name: "Grenade Canister 01"
        Transform {
          Location {
            X: -23.1229382
            Y: -4.74820232
            Z: -7.29148626
          }
          Rotation {
            Roll: 89.9999542
          }
          Scale {
            X: 0.412767261
            Y: 0.412767351
            Z: 0.343740135
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 7409974748338403079
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
        Id: 13198505940496247226
        Name: "Grenade Canister 04"
        Transform {
          Location {
            X: -22.408987
            Z: 2.74519897
          }
          Rotation {
            Pitch: 90
            Yaw: -5.05115461
            Roll: -5.05114746
          }
          Scale {
            X: 0.589667618
            Y: 0.589667559
            Z: 0.956766605
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6855348992067761797
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
        Id: 17386555362833662145
        Name: "Modern Weapon - Scope 02"
        Transform {
          Location {
            X: -3.27711058
            Z: 12.7963161
          }
          Rotation {
          }
          Scale {
            X: 0.532917678
            Y: 0.532917678
            Z: 0.532917678
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 3210710071777820963
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
        Id: 10542624181003926364
        Name: "Modern Weapon Ammo - Cannister 01"
        Transform {
          Location {
            X: -46.6892166
            Z: 2.47443366
          }
          Rotation {
          }
          Scale {
            X: 2.5823257
            Y: 0.755280316
            Z: 0.755280614
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 6777447344140592468
            }
          }
          Overrides {
            Name: "ma:Shared_Detail1:color"
            Color {
              G: 0.389403731
              B: 0.840000033
              A: 1
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 11603742336928010678
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
        Id: 5523085111066544486
        Name: "Grenade Canister 03"
        Transform {
          Location {
            X: 0.6381827
            Y: -5.18456841
            Z: 2.50194836
          }
          Rotation {
            Pitch: -90
            Yaw: 0.000109283021
            Roll: -3.05175781e-05
          }
          Scale {
            X: 0.589667559
            Y: 0.589668334
            Z: 0.964666
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_Detail1:id"
            AssetReference {
              Id: 4690086130386063043
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 2541187401339965626
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
        Id: 8655009364240378567
        Name: "Grenade Canister 03"
        Transform {
          Location {
            X: 0.6381827
            Y: 5.1843524
            Z: 2.50194836
          }
          Rotation {
            Pitch: -90
            Yaw: -0.000109283021
            Roll: 0.000188048463
          }
          Scale {
            X: 0.589667559
            Y: 0.589668334
            Z: 0.964666
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_Detail1:id"
            AssetReference {
              Id: 4690086130386063043
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 2541187401339965626
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
        Id: 12384428516559963586
        Name: "Grenade Canister 03"
        Transform {
          Location {
            X: 0.6381827
            Y: -4.03332376
            Z: -3.60228467
          }
          Rotation {
            Pitch: -90
            Yaw: 5.46415104e-05
          }
          Scale {
            X: 0.589667559
            Y: 0.589668334
            Z: 0.964666
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_Detail1:id"
            AssetReference {
              Id: 4690086130386063043
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 2541187401339965626
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
        Id: 6203210214415092549
        Name: "Grenade Canister 03"
        Transform {
          Location {
            X: 0.6381827
            Z: -7.39301538
          }
          Rotation {
            Pitch: -90
            Yaw: 0.000109283021
            Roll: -6.10351563e-05
          }
          Scale {
            X: 0.589667559
            Y: 0.589668334
            Z: 0.964666
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_Detail1:id"
            AssetReference {
              Id: 4690086130386063043
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 2541187401339965626
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
        Id: 3426068431987705660
        Name: "Grenade Canister 03"
        Transform {
          Location {
            X: 0.6381827
            Y: 4.03332376
            Z: -3.60228467
          }
          Rotation {
            Pitch: -90
            Yaw: 0.000109283021
            Roll: -6.10351563e-05
          }
          Scale {
            X: 0.589667559
            Y: 0.589668334
            Z: 0.964666
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_Detail1:id"
            AssetReference {
              Id: 4690086130386063043
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 2541187401339965626
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
        Id: 12537953230251670080
        Name: "Pipe - 90-Degree Short Thick"
        Transform {
          Location {
            X: 23.7879524
            Y: 4.33922482
            Z: -3.4220624
          }
          Rotation {
            Pitch: 44.9999733
            Yaw: -89.9999771
            Roll: 89.9999542
          }
          Scale {
            X: 0.0618341751
            Y: 0.0618341751
            Z: 0.0618341714
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17223073294780175050
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
        Id: 9328885932881089165
        Name: "Pipe - 90-Degree Short Thick"
        Transform {
          Location {
            X: 23.7879524
            Y: -4.33877468
            Z: -3.4220624
          }
          Rotation {
            Pitch: 45.0000038
            Yaw: 90.0000229
            Roll: -90.0000458
          }
          Scale {
            X: 0.0618341789
            Y: 0.0618341789
            Z: 0.0618341714
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17223073294780175050
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
        Id: 10277425235323576471
        Name: "Pipe - 90-Degree Short Thick"
        Transform {
          Location {
            X: 23.5867023
            Y: 5.427
            Z: 2.67400074
          }
          Rotation {
            Pitch: -3.05175781e-05
            Yaw: -90
            Roll: 89.9999466
          }
          Scale {
            X: 0.0618341863
            Y: 0.0618341863
            Z: 0.0618341714
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17223073294780175050
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
        Id: 3984816766769042986
        Name: "Pipe - 90-Degree Short Thick"
        Transform {
          Location {
            X: 23.5867023
            Y: -5.42664051
            Z: 2.67400074
          }
          Rotation {
            Pitch: 4.78113216e-05
            Yaw: 89.9999847
            Roll: -90.0000305
          }
          Scale {
            X: 0.0618342496
            Y: 0.0618342496
            Z: 0.0618341714
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17223073294780175050
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
        Id: 9239214801683291888
        Name: "Cylinder - Polished"
        Transform {
          Location {
            X: -3.56887627
            Y: -6.78625154
            Z: 9.609128
          }
          Rotation {
            Pitch: 90
          }
          Scale {
            X: 0.0164340101
            Y: 0.0164339878
            Z: 0.0876596048
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 14109598334483438678
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
        Id: 882013316385660966
        Name: "Pipe - 90-Degree Long Thick"
        Transform {
          Location {
            X: -7.75712109
            Y: -6.81378412
            Z: 9.60032749
          }
          Rotation {
            Pitch: -44.999958
            Yaw: 89.9999695
            Roll: 89.9999924
          }
          Scale {
            X: 0.0170065127
            Y: 0.0170065127
            Z: 0.0170065127
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:utile"
            Float: 0.28
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:vtile"
            Float: 0.28
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 16051528326487208434
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
        Id: 8813141093374022262
        Name: "Cylinder - Polished"
        Transform {
          Location {
            X: -3.56887627
            Y: 6.78589153
            Z: 9.609128
          }
          Rotation {
            Pitch: 90
          }
          Scale {
            X: 0.0164340101
            Y: 0.0164339878
            Z: 0.0876596048
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 14109598334483438678
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
        Id: 5457433386744341140
        Name: "Pipe - 90-Degree Long Thick"
        Transform {
          Location {
            X: -7.75712109
            Y: 6.81419802
            Z: 9.60032749
          }
          Rotation {
            Pitch: -44.9999924
            Yaw: -90.0000153
            Roll: -90.0000076
          }
          Scale {
            X: 0.0170065127
            Y: 0.0170065127
            Z: 0.0170065127
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:utile"
            Float: 0.28
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:vtile"
            Float: 0.28
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 16051528326487208434
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
        Id: 15638490643766118142
        Name: "Modern Weapon - Barrel Tip 03"
        Transform {
          Location {
            X: 32.3755341
            Z: 1.15543759
          }
          Rotation {
            Roll: -179.999969
          }
          Scale {
            X: 2.3636682
            Y: 1.64445519
            Z: 1.64445531
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 1329351575637904310
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13382674751763746283
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
        Id: 426172065337851332
        Name: "Grenade Canister 03"
        Transform {
          Location {
            X: 0.6381827
            Z: 7.52144718
          }
          Rotation {
            Pitch: -90
            Yaw: 0.000163924531
            Roll: -9.15527344e-05
          }
          Scale {
            X: 0.589667499
            Y: 0.589668334
            Z: 0.964666
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_Detail1:id"
            AssetReference {
              Id: 4690086130386063043
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 2541187401339965626
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
        Id: 9860088749585857623
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 33.4527
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 18101562252596298307
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 12728615619235702336
        Name: "Pipe - 90-Degree Short Thick"
        Transform {
          Location {
            X: 23.7879524
            Z: 7.31810141
          }
          Rotation {
            Pitch: -90
            Yaw: 125.264381
            Roll: -125.264404
          }
          Scale {
            X: 0.0618341677
            Y: 0.0618341677
            Z: 0.0618341714
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 17223073294780175050
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
        Id: 1523549327104743604
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 37.0081367
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 8733159910986960156
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 14267265505400304807
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 40.5553093
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 8733159910986960156
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 6123106475970478214
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 44.1601486
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 8733159910986960156
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 13224037466502210629
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 58.4836655
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 8733159910986960156
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 9942268804713016823
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 54.8788261
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 8733159910986960156
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 4541501496141212901
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 51.3316612
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 8733159910986960156
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 17289058737757856298
        Name: "Ring - Beveled"
        Transform {
          Location {
            X: 47.7762794
            Z: 1.39100409
          }
          Rotation {
            Pitch: 15.4787445
            Yaw: 89.9999466
            Roll: -89.999939
          }
          Scale {
            X: 0.134022251
            Y: 0.13402243
            Z: 0.258518428
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 8733159910986960156
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 13482740611232334471
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
        Id: 11272703844539274545
        Name: "ForwardRing"
        Transform {
          Location {
            X: 70
            Z: -5
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 972788928337400750
        ChildIds: 3033787705580569319
        ChildIds: 10705695114950547377
        ChildIds: 15153898771196146872
        ChildIds: 12007680353323428218
        ChildIds: 16040230946455622852
        ChildIds: 15719409333077526484
        ChildIds: 11431497765030649568
        ChildIds: 9650085237394462435
        ChildIds: 3899718657548695790
        ChildIds: 12052173118686820030
        ChildIds: 98689516711190759
        ChildIds: 8851328908008276181
        ChildIds: 11763375385748063427
        ChildIds: 16167729756513251045
        ChildIds: 4094971889578041414
        ChildIds: 4395235489525974251
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
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
        Id: 3033787705580569319
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: -1.87531853
            Z: 10.7872305
          }
          Rotation {
            Roll: -22.499939
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 10705695114950547377
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: -3.46514273
            Z: 9.72494507
          }
          Rotation {
            Roll: -44.9999695
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 15153898771196146872
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: -4.52743673
            Z: 8.13512
          }
          Rotation {
            Roll: -67.5000305
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 12007680353323428218
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: -4.90046
            Z: 6.2597928
          }
          Rotation {
            Roll: -90
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 16040230946455622852
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: -4.52743673
            Z: 4.38447428
          }
          Rotation {
            Roll: -112.499939
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 15719409333077526484
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: -3.46516061
            Z: 2.79464984
          }
          Rotation {
            Roll: -134.999878
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 11431497765030649568
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: -1.87533641
            Z: 1.73236454
          }
          Rotation {
            Roll: -157.499939
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 9650085237394462435
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Z: 1.35933244
          }
          Rotation {
            Roll: -179.999893
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 3899718657548695790
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: 1.87531853
            Z: 1.73235559
          }
          Rotation {
            Roll: 157.500107
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 12052173118686820030
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: 3.46514273
            Z: 2.79463196
          }
          Rotation {
            Roll: 135.000122
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 98689516711190759
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: 4.52743673
            Z: 4.38445616
          }
          Rotation {
            Roll: 112.500053
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 8851328908008276181
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: 4.90046
            Z: 6.25978374
          }
          Rotation {
            Roll: 90.0000763
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 11763375385748063427
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: 4.52743673
            Z: 8.13511181
          }
          Rotation {
            Roll: 67.5001373
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 16167729756513251045
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: 3.46516061
            Z: 9.72493553
          }
          Rotation {
            Roll: 45.0001335
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 4094971889578041414
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Y: 1.87533641
            Z: 10.787221
          }
          Rotation {
            Roll: 22.5000668
          }
          Scale {
            X: 2.49999976
            Y: 0.727613747
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 4395235489525974251
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -5.13656616
            Z: 11.1644106
          }
          Rotation {
          }
          Scale {
            X: 2.5
            Y: 0.727613926
            Z: 0.147136897
          }
        }
        ParentId: 11272703844539274545
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 9598404518457013145
        Name: "Modern Weapon - Magazine 03"
        Transform {
          Location {
            X: -3.42978215
            Z: -8.01144791
          }
          Rotation {
          }
          Scale {
            X: 1.24555969
            Y: 2.65795875
            Z: 1.17933524
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 5991572880231223520
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
        Id: 16614677249179763360
        Name: "Grenade Canister 01"
        Transform {
          Location {
            X: -31.1606541
            Y: -6.3825469
            Z: -6.20011187
          }
          Rotation {
            Roll: 89.9999542
          }
          Scale {
            X: 0.589667499
            Y: 0.589667439
            Z: 0.455963433
          }
        }
        ParentId: 972788928337400750
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 7409974748338403079
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
        Id: 18312192889408922643
        Name: "ForwardRing"
        Transform {
          Location {
            X: 70.7601089
            Z: -5.00000095
          }
          Rotation {
            Yaw: 179.999954
          }
          Scale {
            X: 0.999999762
            Y: 0.999999762
            Z: 1
          }
        }
        ParentId: 972788928337400750
        ChildIds: 17483471430878198249
        ChildIds: 1279935865101655540
        ChildIds: 14187332266343724242
        ChildIds: 16608330436954879144
        ChildIds: 1919651037360997825
        ChildIds: 2014007746607190457
        ChildIds: 3791068075473079813
        ChildIds: 9670724299554977777
        ChildIds: 12542787413927873349
        ChildIds: 2260569215266087645
        ChildIds: 3737673859474268109
        ChildIds: 12778359367212102057
        ChildIds: 17475172484783819008
        ChildIds: 16840307652793567944
        ChildIds: 42281414161760987
        ChildIds: 3914735707877306138
        ChildIds: 14168341962031569594
        ChildIds: 18344941134795152758
        ChildIds: 1917254361657321444
        ChildIds: 736019179492661806
        ChildIds: 4671729500653341071
        ChildIds: 1738953631577378064
        ChildIds: 5096490470242272492
        ChildIds: 3658672378894340724
        ChildIds: 2637760152311813847
        ChildIds: 14298021775279843938
        ChildIds: 4024618130181668366
        ChildIds: 7150662532477828833
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
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
        Id: 17483471430878198249
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063276
            Y: -1.87533832
            Z: 10.787221
          }
          Rotation {
            Roll: -22.4998779
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 1279935865101655540
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063157
            Y: -3.4651444
            Z: 9.72495365
          }
          Rotation {
            Roll: -44.9999695
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 14187332266343724242
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50064
            Y: -4.527421
            Z: 8.13512
          }
          Rotation {
            Roll: -67.5000305
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 16608330436954879144
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50064
            Y: -4.90046072
            Z: 6.25980186
          }
          Rotation {
            Roll: -90
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 1919651037360997825
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50064
            Y: -4.527421
            Z: 4.38446522
          }
          Rotation {
            Roll: -112.499893
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 2014007746607190457
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063157
            Y: -3.4651444
            Z: 2.79464984
          }
          Rotation {
            Roll: -134.999863
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 3791068075473079813
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063276
            Y: -1.87533832
            Z: 1.73236454
          }
          Rotation {
            Roll: -157.499939
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 9670724299554977777
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063419
            Y: -1.78859307e-06
            Z: 1.3593415
          }
          Rotation {
            Roll: -179.999893
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 12542787413927873349
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063562
            Y: 1.87533462
            Z: 1.73236454
          }
          Rotation {
            Roll: 157.500107
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 2260569215266087645
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063682
            Y: 3.46515894
            Z: 2.79463196
          }
          Rotation {
            Roll: 135.000122
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 3737673859474268109
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063753
            Y: 4.52741718
            Z: 4.38446522
          }
          Rotation {
            Roll: 112.500023
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 12778359367212102057
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063753
            Y: 4.90045738
            Z: 6.25978374
          }
          Rotation {
            Roll: 90.0000763
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 17475172484783819008
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063753
            Y: 4.52741718
            Z: 8.13512
          }
          Rotation {
            Roll: 67.5001602
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 16840307652793567944
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063682
            Y: 3.46515894
            Z: 9.72493553
          }
          Rotation {
            Roll: 45.0001335
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 42281414161760987
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063562
            Y: 1.87533462
            Z: 10.787221
          }
          Rotation {
            Roll: 22.5000343
          }
          Scale {
            X: 0.499999851
            Y: 0.749999821
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 3914735707877306138
        Name: "Modern Weapon - Sight Forward 02"
        Transform {
          Location {
            X: -2.50063419
            Y: -1.78859307e-06
            Z: 11.1644106
          }
          Rotation {
          }
          Scale {
            X: 0.500000119
            Y: 0.75000006
            Z: 0.147136897
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 6045540826292531006
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
        Id: 14168341962031569594
        Name: "Cylinder"
        Transform {
          Location {
            X: 76.3492889
            Y: 4.23294592
            Z: 1.42290962
          }
          Rotation {
            Pitch: 90
            Yaw: -9.1069187e-06
            Roll: 179.999939
          }
          Scale {
            X: 0.0535980947
            Y: 0.0535980798
            Z: 0.160938784
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15084374405349799883
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
        Id: 18344941134795152758
        Name: "Horn"
        Transform {
          Location {
            X: 84.2061462
            Y: 4.21533394
            Z: 1.3591615
          }
          Rotation {
            Pitch: 84.4235916
            Yaw: -179.999954
            Roll: 1.67585373e-11
          }
          Scale {
            X: 0.0521602593
            Y: 0.0521602705
            Z: 0.115500145
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 11206744860006537518
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
        Id: 1917254361657321444
        Name: "Cylinder"
        Transform {
          Location {
            X: 76.3492889
            Y: 2.74450612
            Z: -1.19745648
          }
          Rotation {
            Pitch: 45.1284523
            Yaw: 90.0004
            Roll: -89.9996338
          }
          Scale {
            X: 0.0535980947
            Y: 0.0535980798
            Z: 0.160938784
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15084374405349799883
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
        Id: 736019179492661806
        Name: "Horn"
        Transform {
          Location {
            X: 84.2061462
            Y: 2.68705297
            Z: -1.2302078
          }
          Rotation {
            Pitch: 44.8572845
            Yaw: 97.8798599
            Roll: -84.4244
          }
          Scale {
            X: 0.0521602593
            Y: 0.0521602705
            Z: 0.115500145
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 11206744860006537518
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
        Id: 4671729500653341071
        Name: "Cylinder"
        Transform {
          Location {
            X: 76.3490219
            Y: -4.233
            Z: 1.42290962
          }
          Rotation {
            Pitch: 90
            Yaw: 1.82138374e-05
            Roll: 179.999939
          }
          Scale {
            X: 0.0535981022
            Y: 0.0535980873
            Z: 0.160938784
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15084374405349799883
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
        Id: 1738953631577378064
        Name: "Horn"
        Transform {
          Location {
            X: 84.1869354
            Y: -4.21499777
            Z: 1.3591615
          }
          Rotation {
            Pitch: 0.590893269
            Yaw: 95.5782852
            Roll: -89.9380493
          }
          Scale {
            X: 0.0521602705
            Y: 0.0521602705
            Z: 0.115500145
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 11206744860006537518
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
        Id: 5096490470242272492
        Name: "Cylinder"
        Transform {
          Location {
            X: 76.3490219
            Y: -2.745
            Z: -1.19745648
          }
          Rotation {
            Pitch: 45.128418
            Yaw: 90.0003891
            Roll: -89.999588
          }
          Scale {
            X: 0.0535981
            Y: 0.0535980836
            Z: 0.160938784
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15084374405349799883
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
        Id: 3658672378894340724
        Name: "Cylinder"
        Transform {
          Location {
            X: 0.371295452
            Y: 2.65571288e-07
            Z: 6.17427969
          }
          Rotation {
            Pitch: -90
            Yaw: 9.1069187e-06
            Roll: -179.999939
          }
          Scale {
            X: 0.0889476314
            Y: 0.0889476314
            Z: 0.0889476165
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 353632070207358285
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:color"
            Color {
              G: 11.125824
              B: 15
              A: 1
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15084374405349799883
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
        Id: 2637760152311813847
        Name: "Horn"
        Transform {
          Location {
            X: 84.2058792
            Y: -2.687
            Z: -1.2302078
          }
          Rotation {
            Pitch: 44.8572617
            Yaw: 97.8798676
            Roll: -84.4243774
          }
          Scale {
            X: 0.0521602631
            Y: 0.0521602705
            Z: 0.115500145
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:smart"
            Bool: false
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 13760639396431168944
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 11206744860006537518
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
        Id: 14298021775279843938
        Name: "Cylinder"
        Transform {
          Location {
            X: -1.00596011
            Y: -7.1951888e-07
            Z: 6.17427969
          }
          Rotation {
            Pitch: -90
            Yaw: 9.1069187e-06
            Roll: -179.999939
          }
          Scale {
            X: 0.106069274
            Y: 0.106069669
            Z: 0.0363099799
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 353632070207358285
            }
          }
          Overrides {
            Name: "ma:Shared_BaseMaterial:color"
            Color {
              G: 11.125824
              B: 15
              A: 1
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 15084374405349799883
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
        Id: 4024618130181668366
        Name: "Modern Weapon - Body 04"
        Transform {
          Location {
            X: 72.6963577
            Y: 4.57763672e-05
            Z: 17.2649841
          }
          Rotation {
            Pitch: -1.67263126
            Yaw: 90.0000687
            Roll: 179.999512
          }
          Scale {
            X: 0.236371353
            Y: -0.321849793
            Z: 0.250725955
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
          Overrides {
            Name: "ma:Shared_Trim:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 1737305543685255753
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
        Id: 7150662532477828833
        Name: "Modern Weapon - Body 04"
        Transform {
          Location {
            X: 79.496994
            Y: 4.57763672e-05
            Z: 17.2649841
          }
          Rotation {
            Pitch: -1.67263126
            Yaw: 90.0000687
            Roll: 179.999527
          }
          Scale {
            X: 0.236371353
            Y: -0.321849793
            Z: 0.250725955
          }
        }
        ParentId: 18312192889408922643
        UnregisteredParameters {
          Overrides {
            Name: "ma:Shared_BaseMaterial:id"
            AssetReference {
              Id: 15375941495231049454
            }
          }
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:inheritfromparent"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:forceon"
        }
        CoreMesh {
          MeshAsset {
            Id: 1737305543685255753
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
        Id: 3865867001883727515
        Name: "Pickup Trigger"
        Transform {
          Location {
            X: 20.0000019
            Z: 0.090065
          }
          Rotation {
          }
          Scale {
            X: 1
            Y: 1
            Z: 1
          }
        }
        ParentId: 1848594543195918722
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
          Interactable: true
          InteractionLabel: "Equip Advanced Rifle"
          TeamSettings {
            IsTeamCollisionEnabled: true
            IsEnemyCollisionEnabled: true
          }
          TriggerShape_v2 {
            Value: "mc:etriggershape:box"
          }
        }
      }
      Objects {
        Id: 6708322448160599002
        Name: "Shoot"
        ParentId: 1848594543195918722
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:forceoff"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Ability {
          IsEnabled: true
          KeyBinding: "ability_primary"
          CastPhaseSettings {
            Duration: 0.03
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
            Facing_V2 {
              Value: "mc:eabilitysetfacing:aim"
            }
          }
          ExecutePhaseSettings {
            Duration: 0.1
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
            Facing_V2 {
              Value: "mc:eabilitysetfacing:aim"
            }
          }
          RecoveryPhaseSettings {
            Duration: 0.01
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
          }
          CooldownPhaseSettings {
            Duration: 0.4
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
          }
          Animation: "2hand_rifle_shoot"
        }
      }
      Objects {
        Id: 5696031600611268807
        Name: "Reload"
        ParentId: 1848594543195918722
        UnregisteredParameters {
        }
        WantsNetworking: true
        Collidable_v2 {
          Value: "mc:ecollisionsetting:forceoff"
        }
        Visible_v2 {
          Value: "mc:evisibilitysetting:forceoff"
        }
        CameraCollidable {
          Value: "mc:ecollisionsetting:inheritfromparent"
        }
        Ability {
          IsEnabled: true
          KeyBinding: "ability_ult"
          CastPhaseSettings {
            Duration: 0.65
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
          }
          ExecutePhaseSettings {
            Duration: 0.15
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
          }
          RecoveryPhaseSettings {
            Duration: 0.01
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
          }
          CooldownPhaseSettings {
            CanMove: true
            CanJump: true
            CanRotate: true
            PreventOtherAbilities: true
            IsTargetDataUpdated: true
          }
          Animation: "2hand_rifle_reload_magazine"
          KeyBinding_v2 {
            Value: "mc:egameaction:extraaction_33"
          }
        }
      }
    }
    Assets {
      Id: 499697514733272876
      Name: "Modern Weapon - Stock 01"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_stock_001"
      }
    }
    Assets {
      Id: 13155471131385409602
      Name: "Modern Weapon - Grip 01"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_grip_001"
      }
    }
    Assets {
      Id: 16965777294932964901
      Name: "Cube - Chamfered Large Polished"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_cube_hq_002"
      }
    }
    Assets {
      Id: 132672053610873933
      Name: "Plastic Matte"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "plastic_matte_001"
      }
    }
    Assets {
      Id: 5781445960335659647
      Name: "Modern Weapon - Body 03"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_body_003"
      }
    }
    Assets {
      Id: 13077624968254610965
      Name: "Modern Weapon - Body 01"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_body_001"
      }
    }
    Assets {
      Id: 14723360564525113759
      Name: "Modern Weapon - Foregrip 02"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_foregrip_002"
      }
    }
    Assets {
      Id: 15095053435971896742
      Name: "Modern Weapon - Grenade Canister 02"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weapons_grenade_indented_001"
      }
    }
    Assets {
      Id: 7409974748338403079
      Name: "Modern Weapon - Grenade Canister 01"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weapons_grenade_simple_001"
      }
    }
    Assets {
      Id: 17443924865729885564
      Name: "Ellipsoid - Truncated Hollow Wedge"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_ellipsoid_hollow_wedge_001"
      }
    }
    Assets {
      Id: 3842114781646480660
      Name: "Metal Frame 03"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "mi_metal_frames_003_uv"
      }
    }
    Assets {
      Id: 17223073294780175050
      Name: "Pipe - 90-Degree Short Thick"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_pipe_curve90_003"
      }
    }
    Assets {
      Id: 13760639396431168944
      Name: "Roof Asphalt 03"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "mi_roof_asphalt_003_uv"
      }
    }
    Assets {
      Id: 6157711858907400962
      Name: "Modern Weapon Accessory - Shell Clip"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_acc_shellclip_001"
      }
    }
    Assets {
      Id: 2817890560817570586
      Name: "Modern Weapon Barrel 01"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_barrel_001"
      }
    }
    Assets {
      Id: 6855348992067761797
      Name: "Modern Weapon - Grenade Canister 04"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weapons_grenade_sphere_001"
      }
    }
    Assets {
      Id: 3210710071777820963
      Name: "Modern Weapon - Scope 02"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_scope_002"
      }
    }
    Assets {
      Id: 11603742336928010678
      Name: "Modern Weapon Ammo - Cannister 01"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_ammo_cann_001"
      }
    }
    Assets {
      Id: 6777447344140592468
      Name: "Metal Steel Brushed"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "mi_stainless-steel_001_uv"
      }
    }
    Assets {
      Id: 2541187401339965626
      Name: "Modern Weapon - Grenade Canister 03"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weapons_grenade_teeth_001"
      }
    }
    Assets {
      Id: 14109598334483438678
      Name: "Cylinder - Polished"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_cylinder_hq_test_001"
      }
    }
    Assets {
      Id: 16051528326487208434
      Name: "Pipe - 90-Degree Long Thick"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_pipe_curve90_005"
      }
    }
    Assets {
      Id: 13382674751763746283
      Name: "Modern Weapon - Barrel Tip 03"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_barreltip_003"
      }
    }
    Assets {
      Id: 1329351575637904310
      Name: "Metal Basic Steel"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "steel_001"
      }
    }
    Assets {
      Id: 13482740611232334471
      Name: "Ring - Beveled"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_ring_faceted_001"
      }
    }
    Assets {
      Id: 18101562252596298307
      Name: "Metal Frame 02"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "mi_metal_frames_002_uv"
      }
    }
    Assets {
      Id: 6045540826292531006
      Name: "Modern Weapon - Sight Forward 02"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_sight_forw_002"
      }
    }
    Assets {
      Id: 5991572880231223520
      Name: "Modern Weapon - Magazine 03"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_mag_003"
      }
    }
    Assets {
      Id: 15084374405349799883
      Name: "Cylinder"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_cylinder_002"
      }
    }
    Assets {
      Id: 11206744860006537518
      Name: "Horn"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_horn_001"
      }
    }
    Assets {
      Id: 353632070207358285
      Name: "Emissive Glow Opaque"
      PlatformAssetType: 2
      PrimaryAsset {
        AssetType: "MaterialAssetRef"
        AssetId: "fxma_opaque_emissive"
      }
    }
    Assets {
      Id: 1737305543685255753
      Name: "Modern Weapon - Body 04"
      PlatformAssetType: 1
      PrimaryAsset {
        AssetType: "StaticMeshAssetRef"
        AssetId: "sm_weap_modern_body_004"
      }
    }
    PrimaryAssetId {
      AssetType: "None"
      AssetId: "None"
    }
  }
  Marketplace {
    Description: "Railgun like weapon complete with vfx and pew pew sound effect."
  }
  SerializationVersion: 78
  DirectlyPublished: true
}
