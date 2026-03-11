using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart
{
 public   class Coder
    {
        public enum EncodingMothord
        {
            Default,
            Unicode,
            UTF8,
            ASCII
        }
        private EncodingMothord _encodingMothord;
        public Coder(EncodingMothord encodingMothord)
        {
            _encodingMothord = encodingMothord;
        }

        //解码
        public string GetEncodingString(byte[] dataBytes,int start,int size)
        {
         //   string a= Encoding.Convert.GetString(dataBytes, start, size);
            switch (_encodingMothord)
            {
                case EncodingMothord.Default:
                    return Encoding.Default.GetString(dataBytes,start,size);                 
                case EncodingMothord.Unicode:
                    return Encoding.Unicode.GetString(dataBytes, start, size);          
                case EncodingMothord.UTF8:                
                    return Encoding.UTF8.GetString(dataBytes, start, size);
                case EncodingMothord.ASCII:              
                    return Encoding.ASCII.GetString(dataBytes, start, size);
                default:         
                throw (new Exception("未定义的编码格式"));
            }
        }

        //string加码

        public byte[] GetTextBytes(string datagram)
        {
            byte[] rbyte = new byte[Encoding.UTF8.GetBytes(datagram).Length];
        
            switch (_encodingMothord)
            {
                case EncodingMothord.Default:
                    return rbyte = Encoding.Default.GetBytes(datagram);
                case EncodingMothord.Unicode:
                    return rbyte = Encoding.Unicode.GetBytes(datagram);
                case EncodingMothord.UTF8:
                    return rbyte = Encoding.UTF8.GetBytes(datagram);
                case EncodingMothord.ASCII:
                    return rbyte = Encoding.ASCII.GetBytes(datagram);
                default:                     
                        throw (new Exception("未定义的编码格式"));
             
            }
        }
    }
}
