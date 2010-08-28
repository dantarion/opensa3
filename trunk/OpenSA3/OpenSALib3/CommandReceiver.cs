using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.PSA;

namespace OpenSALib3
{
    /// <summary>
    /// This is a class used for receive commands from python
    /// </summary>
    public unsafe static class CommandReceiver
    {
        public static CommandList _commandList;//I wanted to use CommandList class, but its constracter demands offsets...


        private static void GenericHandler(byte module,byte id,List<int> param,List<ParameterType> types)
        {
            Command c=new Command(_commandList,module,id,(byte)param.Count);
            for (int i = 0; i < param.Count; i++)
            {
                switch (types[i])
                {
                    case ParameterType.Boolean: c[i] = new BooleanParameter(c, (int)types[i], param[i]); break;
                    case ParameterType.Offset: c[i] = new OffsetParameter(c, (int)types[i], param[i]); break;
                    case ParameterType.Requirement: c[i] = new RequirementParameter(c, (int)types[i], param[i]); break;
                    case ParameterType.Scalar: c[i] = new ScalarParameter(c, (int)types[i], param[i]); break;
                    case ParameterType.Value: c[i] = new ValueParameter(c, (int)types[i], param[i]); break;
                    case ParameterType.Variable: c[i] = new ValueParameter(c, (int)types[i], param[i]); break;
                }
                //c[i] = new Parameter(c, (int)types[i], param[i]);
            }
            _commandList.AddByName(c);
        }

        public static void SynchronousTimer(int Frames)
        {
            var param=new List<int>(){Frames,};
            var types=new List<ParameterType>()
            {
                ParameterType.Scalar
            };
            GenericHandler(0x00,0x01,param,types);
        }
        
        public static void AsynchronousTimer(int Frames)
        {
            var param=new List<int>(){Frames,};
            var types=new List<ParameterType>()
            {
                ParameterType.Scalar
            };
            GenericHandler(0x00,0x02,param,types);
        }
        
        public static void SetLoop(int Iterations)
        {
            var param=new List<int>(){Iterations,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x00,0x04,param,types);
        }
        
        public static void ExecuteLoop()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x05,param,types);
        }
        
        public static void Subroutine(int Offset)
        {
            var param=new List<int>(){Offset,};
            var types=new List<ParameterType>()
            {
                ParameterType.Offset
            };
            GenericHandler(0x00,0x07,param,types);
        }
        
        public static void Return()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x08,param,types);
        }
        
        public static void Goto(int Offset)
        {
            var param=new List<int>(){Offset,};
            var types=new List<ParameterType>()
            {
                ParameterType.Offset
            };
            GenericHandler(0x00,0x09,param,types);
        }
        
        public static void If(int Requirement)
        {
            var param=new List<int>(){Requirement,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement
            };
            GenericHandler(0x00,0x0A,param,types);
        }
        
        public static void If(int Requirement,int Variable1,int Comparison,int Variable2)
        {
            var param=new List<int>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x00,0x0A,param,types);
        }
        
        public static void And(int Requirement,int Variable1,int Comparison,int Variable2)
        {
            var param=new List<int>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x00,0x0B,param,types);
        }
        
        public static void Or(int Requirement,int Variable1,int Comparison,int Variable2)
        {
            var param=new List<int>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x00,0x0C,param,types);
        }
        
        public static void ElseIf(int Requirement,int Variable1,int Comparison,int Variable2)
        {
            var param=new List<int>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x00,0x0D,param,types);
        }
        
        public static void Else()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x0E,param,types);
        }
        
        public static void EndIf()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x0F,param,types);
        }
        
        public static void Switch()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x10,param,types);
        }
        
        public static void Case()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x11,param,types);
        }
        
        public static void DefaultCase()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x12,param,types);
        }
        
        public static void EndSwitch()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x13,param,types);
        }
        
        public static void LoopRest()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x01,0x01,param,types);
        }
        
        //public static void ChangeActionStatus(int /S/tatusID,int ID,int Requirement,int /V/ariable1,int Comparison,int Variable2)
        //{
        //    var param=new List<int>()/{/StatusID,ID,Requirement,Variable1,Comparis//on,Variable2,};
        //    var types=new List<ParameterType>()
        //    {
        //        ParameterType.Value,
        //        ParameterType.Value,
        //        ParameterType.Requirement,
        //        ParameterType
        //        ParameterType
        //        ParameterType
        //    };
        //    GenericHandler(0x02,0x00,param,types);
        //}
        
        public static void ChangeAction(int ID,int Requirement,int Variable1,int Comparison,int Variable2)
        {
            var param=new List<int>(){ID,Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x02,0x01,param,types);
        }
        
        public static void AdditionalRequirement(int Requirement,int Variable1,int Comparison,int Variable2)
        {
            var param=new List<int>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x02,0x04,param,types);
        }
        
        public static void AllowChangeAction(int StatusID)
        {
            var param=new List<int>(){StatusID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x06,param,types);
        }
        
        public static void DisableChangeAction(int StatusID)
        {
            var param=new List<int>(){StatusID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x08,param,types);
        }
        
        public static void SelectiveIASA(int IASA)
        {
            var param=new List<int>(){IASA,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x0A,param,types);
        }
        
        public static void EndSelectiveIASA(int IASA)
        {
            var param=new List<int>(){IASA,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x0B,param,types);
        }
        
        public static void ChangeSubaction(int ID,int PassFrame)
        {
            var param=new List<int>(){ID,PassFrame,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Boolean
            };
            GenericHandler(0x04,0x00,param,types);
        }
        
        public static void FrameSpeed(int Multiplier)
        {
            var param=new List<int>(){Multiplier,};
            var types=new List<ParameterType>()
            {
                ParameterType.Scalar
            };
            GenericHandler(0x04,0x07,param,types);
        }
        
        public static void ReverseDirection()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x05,0x00,param,types);
        }
        
        public static void OffensiveCollision(int BoneID,int Damage,int Trajectory,int BaseKnockback,int KnockbackGrowth,int Size,int ZOffset,int YOffset,int XOffset,int TrippingRate,int HitlagMultiplier,int SDIMultiplier,int HitboxFlags)
        {
            var param=new List<int>(){BoneID,Damage,Trajectory,BaseKnockback,KnockbackGrowth,Size,ZOffset,YOffset,XOffset,TrippingRate,HitlagMultiplier,SDIMultiplier,HitboxFlags,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Value
            };
            GenericHandler(0x06,0x00,param,types);
        }
        
        public static void ChangeHitboxDamage(int HitboxID,int Damage)
        {
            var param=new List<int>(){HitboxID,Damage,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x01,param,types);
        }
        
        public static void ChangeHitboxSize(int HitboxID,int Damage)
        {
            var param=new List<int>(){HitboxID,Damage,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x02,param,types);
        }
        
        public static void DeleteHitbox(int HitboxID,int Damage)
        {
            var param=new List<int>(){HitboxID,Damage,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x03,param,types);
        }
        
        public static void TerminateCollisions()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x04,param,types);
        }
        
        public static void BodyCollision(int CollisionState)
        {
            var param=new List<int>(){CollisionState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x06,0x05,param,types);
        }
        
        public static void ResetBoneCollisions()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x06,param,types);
        }
        
        public static void ModifyBoneCollision(int Bone,int CollisionState)
        {
            var param=new List<int>(){Bone,CollisionState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x08,param,types);
        }
        
        public static void GrabCollision(int ID,int Bone,int Size,int ZOffset,int YOffset,int XOffset,int GrabbedAction,int AirOrGround)
        {
            var param=new List<int>(){ID,Bone,Size,ZOffset,YOffset,XOffset,GrabbedAction,AirOrGround,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Value,
                ParameterType.Value,
            };
            GenericHandler(0x06,0x0A,param,types);
        }
        
        public static void ThrowAttackCollision(int ID,int Bone,int Damage,int Trajectory,int KnockbackGrowth,int WeightKnockback,int BaseKnockback,int Element)
        {
            var param=new List<int>(){ID,Bone,Damage,Trajectory,KnockbackGrowth,WeightKnockback,BaseKnockback,Element,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x0E,param,types);
        }
        
        public static void ThrowCollision()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x0F,param,types);
        }
        
        public static void UninteractiveCollision()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x10,param,types);
        }
        
        public static void SpecialOffensiveCollision()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x15,param,types);
        }
        
        public static void DefensiveCollision()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x17,param,types);
        }
        
        public static void Defensive()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x18,param,types);
        }
        
        public static void WeaponCollision()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x1B,param,types);
        }
        
        public static void ThrownCollision()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x2B,param,types);
        }
        
        public static void Rumble()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x07,0x07,param,types);
        }
        
        public static void RumbleLoop()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x07,0x0B,param,types);
        }
        
        public static void EdgeSticky()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x08,0x00,param,types);
        }
        
        public static void SoundEffect(int ID)
        {
            var param=new List<int>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x00,param,types);
        }
        
        public static void SoundEffect2(int ID)
        {
            var param=new List<int>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x01,param,types);
        }
        
        public static void SoundEffect3(int ID)
        {
            var param=new List<int>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x02,param,types);
        }
        
        public static void StopSoundEffect()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0A,0x03,param,types);
        }
        
        public static void VictorySound()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0A,0x05,param,types);
        }
        
        public static void OtherSoundEffect(int ID)
        {
            var param=new List<int>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x09,param,types);
        }
        
        public static void OtherSoundEffect2(int ID)
        {
            var param=new List<int>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x0A,param,types);
        }
        
        public static void ModelChanger()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0B,0x00,param,types);
        }
        
        public static void Visibility()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0B,0x02,param,types);
        }
        
        public static void TerminateInstance()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x05,param,types);
        }
        
        public static void FinalSmashState()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x06,param,types);
        }
        
        public static void TerminateSelf()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x08,param,types);
        }
        
        public static void EnableOrDisableLedgegrab()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x09,param,types);
        }
        
        public static void LowVoiceClip()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x0B,param,types);
        }
        
        public static void DamageVoiceClip()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x19,param,types);
        }
        
        public static void OttoottoVoiceClip()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x1D,param,types);
        }
        
        public static void TimeManipulation()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x23,param,types);
        }
        
        public static void TagDisplay()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x25,param,types);
        }
        
        public static void ConcurrentInfiniteLoop()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0D,0x00,param,types);
        }
        
        public static void TerminateConcurrentInfiniteLoop()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0D,0x01,param,types);
        }
        
        public static void SetAirOrGround()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x00,param,types);
        }
        
        public static void AddOrSubtractMomentum()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x01,param,types);
        }
        
        public static void VerticalMomentum()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x02,param,types);
        }
        
        public static void HaltVerticalMomentum(int VMState)
        {
            var param=new List<int>(){VMState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x03,param,types);
        }
        
        public static void HorizontalMomentumMod(int HMState)
        {
            var param=new List<int>(){HMState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x04,param,types);
        }
        
        public static void StopHorizontalMomentumMod(int HMState)
        {
            var param=new List<int>(){HMState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x05,param,types);
        }
        
        public static void DisableForce(int VerticalOrHorizontal)
        {
            var param=new List<int>(){VerticalOrHorizontal,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x06,param,types);
        }
        
        public static void ReennableForce(int VerticalOrHorizontal)
        {
            var param=new List<int>(){VerticalOrHorizontal,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x07,param,types);
        }
        
        public static void SetMomentum()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x08,param,types);
        }
        
        public static void GenerateArticleOrProp()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x10,0x00,param,types);
        }
        
        public static void RemoveArticle()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x10,0x03,param,types);
        }
        
        public static void ChangeArticleSubaction(int ArticleID,int ID,int PassFrame)
        {
            var param=new List<int>(){ArticleID,ID,PassFrame,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Scalar
            };
            GenericHandler(0x10,0x04,param,types);
        }
        
        public static void ArticleVisibility(int ArticleID)
        {
            var param=new List<int>(){ArticleID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x10,0x05,param,types);
        }
        
        public static void ArticleVisibility2(int ArticleID)
        {
            var param=new List<int>(){ArticleID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x10,0x07,param,types);
        }
        
        public static void GenerateArticleProp2()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x10,0x0A,param,types);
        }
        
        public static void GraphicEffect(int Bone,int ZOffset,int YOffset,int XOffset,int ZRotation,int YRotation,int XRotation,int Size,int RandomZTrans,int RandomYTrans,int RandomXTrans,int RandomZRot,int RandomYRot,int RandomXRot,int TerminatewithAnimation)
        {
            var param=new List<int>() {       Bone,ZOffset,YOffset,XOffset,ZRotation,YRotation,XRotation,Size,RandomZTrans,RandomYTrans,RandomXTrans,RandomZRot,RandomYRot,RandomXRot,TerminatewithAnimation,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Boolean
            };
            GenericHandler(0x11,0x00,param,types);
        }
        
        public static void ExternalGraphicEffect(int File,int Bone,int ZOffset,int YOffset,int XOffset,int ZRotation,int YRotation,int XRotation,int Size,int Anchored)
        {
            var param=new List<int>(){File,Bone,ZOffset,YOffset,XOffset,ZRotation,YRotation,XRotation,Size,Anchored,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Scalar,
                ParameterType.Boolean
            };
            GenericHandler(0x11,0x01,param,types);
        }
        
        public static void ExternalGraphicEffect2()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x02,param,types);
        }
        
        public static void SwordGlow()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x03,param,types);
        }
        
        public static void TerminateSwordGlow(int FadeTime)
        {
            var param=new List<int>(){FadeTime,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x11,0x05,param,types);
        }
        
        public static void TerminateGraphicEffect()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x15,param,types);
        }
        
        public static void ScreenTint()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x17,param,types);
        }
        
        public static void GenericGraphicEffect()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x1A,param,types);
        }
        
        public static void GenericGraphicEffect2()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x1B,param,types);
        }
        
        public static void BasicVariableSet()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x00,param,types);
        }
        
        public static void BasicVariableAdd()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x01,param,types);
        }
        
        public static void BasicVariableSubtract()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x02,param,types);
        }
        
        public static void FloatVariableSet()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x06,param,types);
        }
        
        public static void FloatVariableAdd()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x07,param,types);
        }
        
        public static void FloatVariableSubtract()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x08,param,types);
        }
        
        public static void BitVariableSet()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x0A,param,types);
        }
        
        public static void BitVariableClear()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x0B,param,types);
        }
        
        public static void StartCombo()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x13,0x00,param,types);
        }
        
        public static void StopAestheticWind()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x14,0x04,param,types);
        }
        
        public static void AestheticWind()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x14,0x07,param,types);
        }
        
        public static void SlopeModelMovement()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x18,0x00,param,types);
        }
        
        public static void Screenshake()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x00,param,types);
        }
        
        public static void SetCameraBoundaries()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x03,param,types);
        }
        
        public static void CameraCloseup()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x04,param,types);
        }
        
        public static void NormalCamera()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x08,param,types);
        }
        
        public static void SuperOrHeavyArmour()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1E,0x00,param,types);
        }
        
        public static void AddOrSubtractDamage()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1E,0x03,param,types);
        }
        
        public static void PickupItem()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x00,param,types);
        }
        
        public static void ThrowItem()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x01,param,types);
        }
        
        public static void DropItem()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x02,param,types);
        }
        
        public static void ConsumeItem()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x03,param,types);
        }
        
        public static void ItemProperty()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x04,param,types);
        }
        
        public static void FireWeapon()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x05,param,types);
        }
        
        public static void FireWeaponProjectile()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x06,param,types);
        }
        
        public static void CrackerLauncher()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x07,param,types);
        }
        
        public static void GenerateItemInHand()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x08,param,types);
        }
        
        public static void ItemVisibility()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x09,param,types);
        }
        
        public static void DestroyHeldItem()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0A,param,types);
        }
        
        public static void BeamSwordTrail()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0C,param,types);
        }
        
        public static void ActivateHeldItem()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0D,param,types);
        }
        
        public static void ThrowItem2()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0E,param,types);
        }
        
        public static void TerminateFlashEffect()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x00,param,types);
        }
        
        public static void FlashOverlayEffect()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x01,param,types);
        }
        
        public static void ChangeFlashOverlayColor()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x02,param,types);
        }
        
        public static void FlashLightEffect()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x05,param,types);
        }
        
        public static void AllowInterrupt()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x64,0x00,param,types);
        }
        
        public static void End()
        {
            var param=new List<int>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x00,param,types);
        }




    }
    public enum Requirements : int
    {
        AnimationEnd=0x1,
        OnGround=0x4,
        InAir=0x4,
        Compare=0x7,
        BitIsSet=0x8,
        FacingRight=0x9,
        FacingLeft=0xA,
        HitBoxConnects=0xB,
        TouchWall=0xC,
        ButtonTap=0xF,
        ArticleExists=0x15,
        ArticleAvailable=0x1C,
        HoldingItem=0x1F,
        RollADie=0x2B,
        ButtonPress=0x30,
        ButtonRelease=0x31,
        ButtonPressed=0x32,
        ButtonNotPressed=0x33,
        HasNotTethered3Times=0x39,
        HasPassedOverLedge=0x3A,
        FacingAwayFromLedge=0x3B
    }
}
