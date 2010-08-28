using System;
using System.Collections.Generic;
using OpenSALib3.PSA;
namespace OpenSALib3 {
    public struct Data {
        public readonly byte Module;
        public readonly byte ID;
        public readonly byte ParameterCount;
        //public readonly byte Unknown;
        //public readonly bint ParameterOffset;
        public Data(byte module, byte id, byte parameterCount = (byte)0) {
            Module = module;
            ID = id;
            ParameterCount = parameterCount;
        }
    }
    /// <summary>
    /// This is a class used to receive commands from Python
    /// </summary>
    public static class CommandReceiver {
        //TODO: This should be explicitly linked somehow (data type, map?), and not publicly settable.
        public static List<Data> CommandList { get; set; } //NOTE: I wanted to use CommandList class, but its constructer demands offsets...
        public static Dictionary<int, Dictionary<ParameterType, object>> Parameter { get; set; } //NOTE: Key is same as CommandList
       
        private static void GenericHandler(Data data, IList<ParameterType> parameters = null, IList<object> values = null) {
            CommandList.Add(data);
            if (parameters == null || values == null) return;
            if (data.ParameterCount != parameters.Count ||
                data.ParameterCount != values.Count)
                throw new ArgumentException("Number of parameters and/or values do not match the number of parameters in Data.");

            var index = CommandList.Count - 1;
            var p = new Dictionary<ParameterType, object>();
            for (var i = 0; i < parameters.Count; i++)
                p.Add(parameters[i], values[i]);
            Parameter.Add(index, p);
        }

        public static void STimer(int frames) {
            var data = new Data(0, 1, 1);
            var parameters = new List<ParameterType> { ParameterType.Scalar };
            var values = new List<object> { frames };
            GenericHandler(data, parameters, values);
        }

        public static void Nop() {
            var data = new Data(0, 2);
            GenericHandler(data);
        }

        public static void ATimer(int frames) {
            var data = new Data(0, 2, 1);
            var parameters = new List<ParameterType> { ParameterType.Scalar };
            var values = new List<object> { frames };
            GenericHandler(data, parameters, values);
        }

        public static void SetLoop(int iterations) {
            var data = new Data(0, 4, 1);
            var parameters = new List<ParameterType> { ParameterType.Value };
            var values = new List<object> { iterations };
            GenericHandler(data, parameters, values);
        }
    }
    public enum Requirements {
        AnimationEnd = 0x01,
        OnGround = 0x03,
        InAir = 0x04,
        Compare = 0x07,
        BitIsSet = 0x08,
        FacingRight = 0x09,
        FacingLeft = 0x0A,
        HitBoxConnects = 0x0B,
        TouchWall = 0x0C,
        ButtonTap = 0x0F,
        ArticleExists = 0x15,
        ArticleAvailable = 0x1C,
        HoldingItem = 0x1F,
        RollADie = 0x2B,
        ButtonPress = 0x30,
        ButtonRelease = 0x31,
        ButtonPressed = 0x32,
        ButtonNotPressed = 0x33,
        HasNotTethered3Times = 0x39,
        HasPassedOverLedge = 0x3A,
        FacingAwayFromLedge = 0x3B
    }
}
