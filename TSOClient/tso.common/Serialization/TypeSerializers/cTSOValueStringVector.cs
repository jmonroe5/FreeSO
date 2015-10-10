﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mina.Core.Buffer;

namespace FSO.Common.Serialization.TypeSerializers
{
    public class cTSOValueStringVector : ITypeSerializer
    {
        private readonly uint CLSID = 0x8973849E;

        public bool CanDeserialize(uint clsid)
        {
            return clsid == CLSID;
        }

        public bool CanSerialize(Type type)
        {
            return type.IsAssignableFrom(typeof(IList<string>));
        }

        public object Deserialize(uint clsid, IoBuffer input, ISerializationContext serializer)
        {
            var result = new List<String>();
            var count = input.GetUInt32();
            for(int i=0; i < count; i++){
                result.Add(IoBufferUtils.GetPascalVLCString(input));
            }
            return result;
        }
        
        public void Serialize(IoBuffer output, object value, ISerializationContext serializer)
        {
            IList<String> list = (IList<String>)value;
            output.PutUInt32((uint)list.Count);
            for(int i=0; i < list.Count; i++){
                output.PutPascalVLCString(list[i]);
            }
        }

        public uint? GetClsid(object value)
        {
            return CLSID;
        }
    }
}