﻿using System.Text;
using System;

namespace SocketLearn
{
    public class Message
    {

        public byte[] dataBytes = new byte[1024];

        public int restDataLength;

        public int startLenght;


        public Message()
        {
            restDataLength = dataBytes.Length - startLenght;
        }

        public byte[] PackData(string data)
        {
            byte[] tempData = Encoding.UTF8.GetBytes(data);
            int dataLength = tempData.Length;
            byte[] lenghtBytes = BitConverter.GetBytes(dataLength);
            byte[] realBytes = new byte[tempData.Length + lenghtBytes.Length];
            lenghtBytes.CopyTo(realBytes, 0);
            tempData.CopyTo(realBytes, lenghtBytes.Length);
            return realBytes;
        }

        public void ParseData(int lenght,Action<string> callback)
        {
            startLenght += lenght;

            while (true)
            {
                if (dataBytes.Length < 4) return;

                int count = BitConverter.ToInt32(dataBytes, 0);
                if ((startLenght - count) >= 4)
                {
                    startLenght = 0;

                    callback(Encoding.UTF8.GetString(dataBytes,4,count));
                }
                else
                {
                    return;
                }
            }
        }
    }
}
