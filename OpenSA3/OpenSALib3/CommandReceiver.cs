﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSALib3.PSA;

namespace OpenSALib3
{
    public struct Data
    {
        public byte Module;
        public byte ID;
        public byte ParameterCount;
        public byte Unknown;
        public bint ParameterOffset;
    }
    /// <summary>
    /// This is a class used for receive commands from python
    /// </summary>
    public unsafe static class CommandReceiver
    {
        private static List<Data> _commandList;//I wanted to use CommandList class, but its constracter demands offsets...
        private static Dictionary<int,Dictionary<ParameterType,object>> _parameterList;//Key is same to CommandList
        public static List<Data> CommandList { get { return _commandList; } set { _commandList = value; } }
        public static  Dictionary<int,Dictionary<ParameterType,object>> Parameter { get { return _parameterList; } set { _parameterList = value; } }


        private static void GenericHandler(Data data, List<ParameterType> pList, List<object> values)
        {
            _commandList.Add(data);
            if (pList != null && values != null)
            {
                if (pList.Count != values.Count)
                    throw new Exception("Invalid value count");

                int index = _commandList.Count - 1;
                Dictionary<ParameterType, object> p = new Dictionary<ParameterType, object>();
                for (int i = 0; i < pList.Count; i++)
                    p.Add(pList[i], values[i]);
                _parameterList.Add(index, p);
            }
        }

        public static void STimer(int frames) 
        {
            Data data = new Data() { Module = 0, ID = 1, ParameterCount = 1 };
            List<ParameterType> pList =new List<ParameterType> 
            { ParameterType.Scalar };
            List<object> values = new List<object> 
            { 
                frames 
            };

            GenericHandler(data, pList, values);
        }

        public static void Nop()
        {
            Data data = new Data() { Module = 0, ID = 2 };
            GenericHandler(data, null, null);
        }

        public static void ATimer(int frames)
        {
            Data data = new Data() { Module = 0, ID = 2, ParameterCount = 1 };
            List<ParameterType> pList = new List<ParameterType>
            {
                ParameterType.Scalar
            };
            List<object> values = new List<object>
            {
                frames
            };

            GenericHandler(data, pList, values);
        }

        public static void SetLoop(int iterations)
        {
            Data data = new Data() { Module = 0, ID = 4, ParameterCount = 1 };
            List<ParameterType> pList = new List<ParameterType>
            {
                ParameterType.Value
            };
            List<object> values = new List<object>
            {
                iterations
            };

            GenericHandler(data, pList, values);
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