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
        public static CommandList _commandList;//I wanted to use CommandList class, but its constractor demands offsets...


        private static void GenericHandler(byte module,byte id,List<object> param,List<ParameterType> types)
        {
            Command c=new Command(_commandList,module,id,(byte)param.Count);
            for (int i = 0; i < param.Count; i++)
            {
                switch (types[i])
                {
                    case ParameterType.Boolean: c[i] = new BooleanParameter(c, (int)types[i],BitConverter.ToInt32(BitConverter.GetBytes((bool)param[i]),0)); break;
                    case ParameterType.Offset: c[i] = new OffsetParameter(c, (int)types[i], BitConverter.ToInt32(BitConverter.GetBytes((int)param[i]), 0)); break;
                    case ParameterType.Requirement: c[i] = new RequirementParameter(c, BitConverter.ToInt32(BitConverter.GetBytes((int)param[i]), 0)); break;
                    case ParameterType.Scalar: c[i] = new ScalarParameter(c, (int)types[i], BitConverter.ToInt32(BitConverter.GetBytes((float)param[i]), 0)); break;
                    case ParameterType.Value: c[i] = new ValueParameter(c, (int)types[i], BitConverter.ToInt32(BitConverter.GetBytes((int)param[i]), 0)); break;
                    case ParameterType.Variable: c[i] = new ValueParameter(c, (int)types[i], BitConverter.ToInt32(BitConverter.GetBytes((int)param[i]), 0)); break;
                }
                //c[i] = new Parameter(c, (int)types[i], param[i]);
            }
            _commandList.AddByName(c);
        }
        #region Commands
        public static void SynchronousTimer(float Frames)
        {
            var param=new List<object>(){Frames,};
            var types=new List<ParameterType>()
            {
                ParameterType.Scalar
            };
            GenericHandler(0x00,0x01,param,types);
        }
        
        public static void AsynchronousTimer(float Frames)
        {
            var param=new List<object>(){Frames,};
            var types=new List<ParameterType>()
            {
                ParameterType.Scalar
            };
            GenericHandler(0x00,0x02,param,types);
        }
        
        public static void SetLoop(int Iterations)
        {
            var param=new List<object>(){Iterations,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x00,0x04,param,types);
        }
        
        public static void ExecuteLoop()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x05,param,types);
        }
        
        public static void Subroutine(int Offset)
        {
            var param=new List<object>(){Offset,};
            var types=new List<ParameterType>()
            {
                ParameterType.Offset
            };
            GenericHandler(0x00,0x07,param,types);
        }
        
        public static void Return()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x08,param,types);
        }
        
        public static void Goto(int Offset)
        {
            var param=new List<object>(){Offset,};
            var types=new List<ParameterType>()
            {
                ParameterType.Offset
            };
            GenericHandler(0x00,0x09,param,types);
        }
        
        public static void If(string requirement)
        {
            int Requirement=0;
            foreach (var n in Utility.PSANames.ReqNames)
                if (n.Value == requirement)
                    Requirement = n.Key;
            var param=new List<object>(){Requirement,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement
            };
            GenericHandler(0x00,0x0A,param,types);
        }
        
        public static void If(string requirement,int Variable1,int Comparison,int Variable2)
        {
            int Requirement = 0;
            foreach (var n in Utility.PSANames.ReqNames)
                if (n.Value == requirement)
                    Requirement = n.Key;
            var param=new List<object>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x00,0x0A,param,types);
        }
        
        public static void And(string requirement,int Variable1,int Comparison,int Variable2)
        {
            int Requirement = 0;
            foreach (var n in Utility.PSANames.ReqNames)
                if (n.Value == requirement)
                    Requirement = n.Key;
            var param=new List<object>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x00,0x0B,param,types);
        }
        
        public static void Or(string requirement,int Variable1,int Comparison,int Variable2)
        {
            int Requirement = 0;
            foreach (var n in Utility.PSANames.ReqNames)
                if (n.Value == requirement)
                    Requirement = n.Key;
            var param=new List<object>(){Requirement,Variable1,Comparison,Variable2,};
            var types=new List<ParameterType>()
            {
                ParameterType.Requirement,
                ParameterType.Variable,
                ParameterType.Value,
                ParameterType.Variable
            };
            GenericHandler(0x00,0x0C,param,types);
        }
        
        public static void ElseIf(string requirement,int Variable1,int Comparison,int Variable2)
        {
            int Requirement = 0;
            foreach (var n in Utility.PSANames.ReqNames)
                if (n.Value == requirement)
                    Requirement = n.Key;
            var param=new List<object>(){Requirement,Variable1,Comparison,Variable2,};
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
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x0E,param,types);
        }
        
        public static void EndIf()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x0F,param,types);
        }
        
        public static void Switch()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x10,param,types);
        }
        
        public static void Case()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x11,param,types);
        }
        
        public static void DefaultCase()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x12,param,types);
        }
        
        public static void EndSwitch()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x13,param,types);
        }
        
        public static void LoopRest()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x01,0x01,param,types);
        }
        
        //public static void ChangeActionStatus(int /S/tatusID,int ID,int Requirement,int /V/ariable1,int Comparison,int Variable2)
        //{
        //    var param=new List<object>()/{/StatusID,ID,Requirement,Variable1,Comparis//on,Variable2,};
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
        
        public static void ChangeAction(int ID,string requirement,int Variable1,int Comparison,int Variable2)
        {
            int Requirement = 0;
            foreach (var n in Utility.PSANames.ReqNames)
                if (n.Value == requirement)
                    Requirement = n.Key;
            var param=new List<object>(){ID,Requirement,Variable1,Comparison,Variable2,};
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
        
        public static void AdditionalRequirement(string requirement,int Variable1,int Comparison,int Variable2)
        {
            int Requirement = 0;
            foreach (var n in Utility.PSANames.ReqNames)
                if (n.Value == requirement)
                    Requirement = n.Key;
            var param=new List<object>(){Requirement,Variable1,Comparison,Variable2,};
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
            var param=new List<object>(){StatusID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x06,param,types);
        }
        
        public static void DisableChangeAction(int StatusID)
        {
            var param=new List<object>(){StatusID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x08,param,types);
        }
        
        public static void SelectiveIASA(int IASA)
        {
            var param=new List<object>(){IASA,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x0A,param,types);
        }
        
        public static void EndSelectiveIASA(int IASA)
        {
            var param=new List<object>(){IASA,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x02,0x0B,param,types);
        }
        
        public static void ChangeSubaction(int ID,bool PassFrame)
        {
            var param=new List<object>(){ID,PassFrame,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Boolean
            };
            GenericHandler(0x04,0x00,param,types);
        }
        
        public static void FrameSpeed(float Multiplier)
        {
            var param=new List<object>(){Multiplier,};
            var types=new List<ParameterType>()
            {
                ParameterType.Scalar
            };
            GenericHandler(0x04,0x07,param,types);
        }
        
        public static void ReverseDirection()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x05,0x00,param,types);
        }
        
        public static void OffensiveCollision(int BoneID,int Damage,int Trajectory,int BaseKnockback,int KnockbackGrowth,float Size,float ZOffset,float YOffset,float XOffset,float TrippingRate,float HitlagMultiplier,float SDIMultiplier,int HitboxFlags)
        {
            var param=new List<object>(){BoneID,Damage,Trajectory,BaseKnockback,KnockbackGrowth,Size,ZOffset,YOffset,XOffset,TrippingRate,HitlagMultiplier,SDIMultiplier,HitboxFlags,};
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
            var param=new List<object>(){HitboxID,Damage,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x01,param,types);
        }
        
        public static void ChangeHitboxSize(int HitboxID,int Damage)
        {
            var param=new List<object>(){HitboxID,Damage,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x02,param,types);
        }
        
        public static void DeleteHitbox(int HitboxID,int Damage)
        {
            var param=new List<object>(){HitboxID,Damage,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x03,param,types);
        }
        
        public static void TerminateCollisions()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x04,param,types);
        }
        
        public static void BodyCollision(int CollisionState)
        {
            var param=new List<object>(){CollisionState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x06,0x05,param,types);
        }
        
        public static void ResetBoneCollisions()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x06,param,types);
        }
        
        public static void ModifyBoneCollision(int Bone,int CollisionState)
        {
            var param=new List<object>(){Bone,CollisionState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value,
                ParameterType.Value
            };
            GenericHandler(0x06,0x08,param,types);
        }
        
        public static void GrabCollision(int ID,int Bone,float Size,float ZOffset,float YOffset,float XOffset,int GrabbedAction,int AirOrGround)
        {
            var param=new List<object>(){ID,Bone,Size,ZOffset,YOffset,XOffset,GrabbedAction,AirOrGround,};
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
            var param=new List<object>(){ID,Bone,Damage,Trajectory,KnockbackGrowth,WeightKnockback,BaseKnockback,Element,};
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
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x0F,param,types);
        }
        
        public static void UninteractiveCollision()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x10,param,types);
        }
        
        public static void SpecialOffensiveCollision()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x15,param,types);
        }
        
        public static void DefensiveCollision()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x17,param,types);
        }
        
        public static void Defensive()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x18,param,types);
        }
        
        public static void WeaponCollision()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x1B,param,types);
        }
        
        public static void ThrownCollision()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x06,0x2B,param,types);
        }
        
        public static void Rumble()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x07,0x07,param,types);
        }
        
        public static void RumbleLoop()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x07,0x0B,param,types);
        }
        
        public static void EdgeSticky()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x08,0x00,param,types);
        }
        
        public static void SoundEffect(int ID)
        {
            var param=new List<object>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x00,param,types);
        }
        
        public static void SoundEffect2(int ID)
        {
            var param=new List<object>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x01,param,types);
        }
        
        public static void SoundEffect3(int ID)
        {
            var param=new List<object>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x02,param,types);
        }
        
        public static void StopSoundEffect()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0A,0x03,param,types);
        }
        
        public static void VictorySound()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0A,0x05,param,types);
        }
        
        public static void OtherSoundEffect(int ID)
        {
            var param=new List<object>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x09,param,types);
        }
        
        public static void OtherSoundEffect2(int ID)
        {
            var param=new List<object>(){ID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0A,0x0A,param,types);
        }
        
        public static void ModelChanger()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0B,0x00,param,types);
        }
        
        public static void Visibility()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0B,0x02,param,types);
        }
        
        public static void TerminateInstance()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x05,param,types);
        }
        
        public static void FinalSmashState()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x06,param,types);
        }
        
        public static void TerminateSelf()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x08,param,types);
        }
        
        public static void EnableOrDisableLedgegrab()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x09,param,types);
        }
        
        public static void LowVoiceClip()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x0B,param,types);
        }
        
        public static void DamageVoiceClip()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x19,param,types);
        }
        
        public static void OttoottoVoiceClip()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x1D,param,types);
        }
        
        public static void TimeManipulation()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x23,param,types);
        }
        
        public static void TagDisplay()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0C,0x25,param,types);
        }
        
        public static void ConcurrentInfiniteLoop()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0D,0x00,param,types);
        }
        
        public static void TerminateConcurrentInfiniteLoop()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0D,0x01,param,types);
        }
        
        public static void SetAirOrGround()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x00,param,types);
        }
        
        public static void AddOrSubtractMomentum()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x01,param,types);
        }
        
        public static void VerticalMomentum()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x02,param,types);
        }
        
        public static void HaltVerticalMomentum(int VMState)
        {
            var param=new List<object>(){VMState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x03,param,types);
        }
        
        public static void HorizontalMomentumMod(int HMState)
        {
            var param=new List<object>(){HMState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x04,param,types);
        }
        
        public static void StopHorizontalMomentumMod(int HMState)
        {
            var param=new List<object>(){HMState,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x05,param,types);
        }
        
        public static void DisableForce(int VerticalOrHorizontal)
        {
            var param=new List<object>(){VerticalOrHorizontal,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x06,param,types);
        }
        
        public static void ReennableForce(int VerticalOrHorizontal)
        {
            var param=new List<object>(){VerticalOrHorizontal,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x0E,0x07,param,types);
        }
        
        public static void SetMomentum()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x0E,0x08,param,types);
        }
        
        public static void GenerateArticleOrProp()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x10,0x00,param,types);
        }
        
        public static void RemoveArticle()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x10,0x03,param,types);
        }
        
        public static void ChangeArticleSubaction(int ArticleID,int ID,float PassFrame)
        {
            var param=new List<object>(){ArticleID,ID,PassFrame,};
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
            var param=new List<object>(){ArticleID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x10,0x05,param,types);
        }
        
        public static void ArticleVisibility2(int ArticleID)
        {
            var param=new List<object>(){ArticleID,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x10,0x07,param,types);
        }
        
        public static void GenerateArticleProp2()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x10,0x0A,param,types);
        }
        
        public static void GraphicEffect(int Bone,float ZOffset,float YOffset,float XOffset,float ZRotation,float YRotation,float XRotation,float Size,float RandomZTrans,float RandomYTrans,float RandomXTrans,float RandomZRot,float RandomYRot,float RandomXRot,bool TerminatewithAnimation)
        {
            var param=new List<object>() {       Bone,ZOffset,YOffset,XOffset,ZRotation,YRotation,XRotation,Size,RandomZTrans,RandomYTrans,RandomXTrans,RandomZRot,RandomYRot,RandomXRot,TerminatewithAnimation,};
            var types=new List<ParameterType>()
            {
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
        
        public static void ExternalGraphicEffect(int File,int Bone,float ZOffset,float YOffset,float XOffset,float ZRotation,float YRotation,float XRotation,float Size,bool Anchored)
        {
            var param=new List<object>(){File,Bone,ZOffset,YOffset,XOffset,ZRotation,YRotation,XRotation,Size,Anchored,};
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
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x02,param,types);
        }
        
        public static void SwordGlow()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x03,param,types);
        }
        
        public static void TerminateSwordGlow(int FadeTime)
        {
            var param=new List<object>(){FadeTime,};
            var types=new List<ParameterType>()
            {
                ParameterType.Value
            };
            GenericHandler(0x11,0x05,param,types);
        }
        
        public static void TerminateGraphicEffect()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x15,param,types);
        }
        
        public static void ScreenTint()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x17,param,types);
        }
        
        public static void GenericGraphicEffect()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x1A,param,types);
        }
        
        public static void GenericGraphicEffect2()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x11,0x1B,param,types);
        }
        
        public static void BasicVariableSet()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x00,param,types);
        }
        
        public static void BasicVariableAdd()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x01,param,types);
        }
        
        public static void BasicVariableSubtract()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x02,param,types);
        }
        
        public static void FloatVariableSet()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x06,param,types);
        }
        
        public static void FloatVariableAdd()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x07,param,types);
        }
        
        public static void FloatVariableSubtract()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x08,param,types);
        }
        
        public static void BitVariableSet()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x0A,param,types);
        }
        
        public static void BitVariableClear()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x12,0x0B,param,types);
        }
        
        public static void StartCombo()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x13,0x00,param,types);
        }
        
        public static void StopAestheticWind()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x14,0x04,param,types);
        }
        
        public static void AestheticWind()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x14,0x07,param,types);
        }
        
        public static void SlopeModelMovement()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x18,0x00,param,types);
        }
        
        public static void Screenshake()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x00,param,types);
        }
        
        public static void SetCameraBoundaries()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x03,param,types);
        }
        
        public static void CameraCloseup()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x04,param,types);
        }
        
        public static void NormalCamera()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1A,0x08,param,types);
        }
        
        public static void SuperOrHeavyArmour()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1E,0x00,param,types);
        }
        
        public static void AddOrSubtractDamage()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1E,0x03,param,types);
        }
        
        public static void PickupItem()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x00,param,types);
        }
        
        public static void ThrowItem()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x01,param,types);
        }
        
        public static void DropItem()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x02,param,types);
        }
        
        public static void ConsumeItem()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x03,param,types);
        }
        
        public static void ItemProperty()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x04,param,types);
        }
        
        public static void FireWeapon()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x05,param,types);
        }
        
        public static void FireWeaponProjectile()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x06,param,types);
        }
        
        public static void CrackerLauncher()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x07,param,types);
        }
        
        public static void GenerateItemInHand()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x08,param,types);
        }
        
        public static void ItemVisibility()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x09,param,types);
        }
        
        public static void DestroyHeldItem()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0A,param,types);
        }
        
        public static void BeamSwordTrail()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0C,param,types);
        }
        
        public static void ActivateHeldItem()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0D,param,types);
        }
        
        public static void ThrowItem2()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x1F,0x0E,param,types);
        }
        
        public static void TerminateFlashEffect()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x00,param,types);
        }
        
        public static void FlashOverlayEffect()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x01,param,types);
        }
        
        public static void ChangeFlashOverlayColor()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x02,param,types);
        }
        
        public static void FlashLightEffect()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x21,0x05,param,types);
        }
        
        public static void AllowInterrupt()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x64,0x00,param,types);
        }
        
        public static void End()
        {
            var param=new List<object>(){};
            var types=new List<ParameterType>()
            {
            };
            GenericHandler(0x00,0x00,param,types);
        }
        #endregion
        #region User Support Functions
        #region Variable Converters
        public static int ICBasic(int index)
        {return 0x00000000 + index;}

        public static int LABasic(int index)
        {return 0x10000000 + index;}

        public static int RABasic(int index)
        {return 0x20000000 + index;}

        public static int ICFloat(int index)
        {return 0x01000000 + index;}

        public static int LAFloat(int index)
        { return 0x11000000 + index;}

        public static int RAFloat(int index)
        {return 0x21000000 + index;}

        public static int ICBit(int index)
        {return 0x02000000 + index;}

        public static int LABit(int index)
        {return 0x12000000 + index;}

        public static int RABit(int index)
        {return 0x22000000 + index; }
        #endregion
        //TODO Make a method to count all used variables in moveset file.
        #region VariableGetter
        private static List<int> UsedICBasic = new List<int>() { 2, 8, 1010, 1011, 1012, 1013, 1014, 3193, 20000, 20001, 20002, 20003 };
        private static List<int> UsedICFloat = new List<int>();
        private static List<int> UsedICBit = new List<int>();
        private static List<int> UsedLABit = new List<int>() { 0, 1, 8, 9, 27, 57,61, 65,255 };
        private static List<int> UsedLABasic = new List<int>() { 1, 3, 4, 13, 24, 25, 26, 30, 33, 34, 35, 37, 44, 53 };
        private static List<int> UsedLAFloat = new List<int>() { 3, 7, 8 };
        private static List<int> UsedRABit = new List<int>() { 2, 16,17, 18, 19, 20,21, 22, 25, 30 };
        private static List<int> UsedRABasic = new List<int>() { 14};
        private static List<int> UsedRAFloat = new List<int>();
        //public static int GetUsableICBasic()
        //{
        //    UsedICBasic.Sort();
        //    var i = 0;
        //    while (UsedICBasic.Contains(i))
        //        i++;
        //    UsedICBacic.Add(i);
        //    return i;
        //}
        #endregion
        public static int SetFlag(string FlagNames)
        {
            var flags = FlagNames.Split('|');
            var resultFlag = 0;
            foreach(var flag in flags)
            {
                int temp=0;
                Utility.PSANames.FlagNames.TryGetValue(flag,out temp);
                resultFlag += temp;
            }
            return resultFlag;
        }
        
        #endregion

    }

}
