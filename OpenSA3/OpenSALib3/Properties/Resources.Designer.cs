﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.1
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenSALib3.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OpenSALib3.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   0x000 Walk Initial Velocity
        ///The starting velocity obtained the moment the character starts walking.
        ///0
        ///
        ///0x004 Walk Acceleration
        ///The speed of acceleration while walking.
        ///0
        ///
        ///0x008 Walk Maximum Velocity
        ///The maximum velocity obtainable while walking.
        ///0
        ///
        ///0x00C Stopping Velocity
        ///The speed at which the character is able to stop at.
        ///0
        ///
        ///0x010 Dash &amp; StopTurn Initial Velocity
        ///The starting velocity obtained the moment the character starts a Dash.
        ///0
        ///
        ///0x014 StopTurn Deceleration
        ///The speed at which th [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Attributes {
            get {
                return ResourceManager.GetString("Attributes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   0001
        ///SynchronousTimer
        ///Pause the current flow of events until the set time is reached. Synchronous timers count down when they are reached in the code. 
        ///Frames
        ///F:SynchronousTimer(Frames={0})
        ///
        ///0002
        ///AsynchronousTimer
        ///Pause the current flow of events until the set time is reached. Asynchronous Timers start counting from the beginning of the animation.
        ///Frames
        ///F:AsynchronousTimer(Frames={0})
        ///
        ///0004
        ///SetLoop
        ///Set a loop for X iterations. -1 (0xFFFF) sets an infinite loop.
        ///Iterations
        ///F:SetLoop(Iteratio [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Events {
            get {
                return ResourceManager.GetString("Events", resourceCulture);
            }
        }
        
        /// <summary>
        ///   10000000
        ///Transcendent1
        ///
        ///22000000
        ///Transcendent2
        ///
        ///32000000
        ///Transcendent3
        ///
        ///36000000
        ///Transcendent4
        ///
        ///10000
        ///OnlyGround
        ///
        ///20000
        ///OnlyAir
        ///
        ///30000
        ///Both
        ///
        ///00
        ///NormalHit
        ///
        ///01
        ///Nothing
        ///
        ///02
        ///Slash
        ///
        ///03
        ///Electricity
        ///
        ///04
        ///Ice
        ///
        ///05
        ///Flame
        ///
        ///06
        ///Coin
        ///
        ///07
        ///Cape
        ///
        ///08
        ///Slip
        ///
        ///09
        ///Sleep
        ///
        ///0A
        ///Nothing
        ///
        ///0B
        ///Impale
        ///
        ///0C
        ///Stun
        ///
        ///0D
        ///Unknown
        ///
        ///0E
        ///Flower
        ///
        ///11
        ///Slash2
        ///
        ///12
        ///Hit2
        ///
        ///13
        ///Darkness
        ///
        ///14
        ///Stun
        ///
        ///15
        ///Aura
        ///
        ///16
        ///Impale2
        ///
        ///17
        ///Down
        ///
        ///18
        ///No Flinch  に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string HitboxFlags {
            get {
                return ResourceManager.GetString("HitboxFlags", resourceCulture);
            }
        }
        
        /// <summary>
        ///   00
        ///Animation End
        ///02
        ///On Ground
        ///In Air
        ///05
        ///06
        ///Compare
        ///Bit is Set
        ///Facing Right
        ///Facing Left
        ///Hitbox Connects
        ///Touch Stage
        ///0D
        ///0E
        ///Item Death?
        ///00
        ///11
        ///12
        ///13
        ///14
        ///Article Exists
        ///16
        ///17
        ///Article?(ID,?)
        ///19
        ///1A
        ///1B
        ///Article Limit
        ///1D
        ///1E
        ///Holding Item
        ///20
        ///21
        ///22
        ///23
        ///24
        ///25
        ///26
        ///27
        ///28
        ///29
        ///2A
        ///Random
        ///2C
        ///2D
        ///2E
        ///2F
        ///Button Press
        ///Button Released
        ///Button Held
        ///Button Not Held
        ///34
        ///35
        ///36
        ///37
        ///38
        ///Hasn&apos;t Tethered 3 times
        ///Passed over ledge
        ///Passed over ledge(facing away)
        ///3C
        ///3D
        ///3E
        ///3F
        ///40
        ///41
        ///42
        ///4 [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Requirements {
            get {
                return ResourceManager.GetString("Requirements", resourceCulture);
            }
        }
    }
}
