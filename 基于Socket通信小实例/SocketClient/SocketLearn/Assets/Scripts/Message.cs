﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class Message {

    public byte[] dataBytes = new byte[1024];

    public int restDataLength;

    public int startLenght;

    public string recieveData = "";

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
        lenghtBytes.CopyTo(realBytes,0);
        tempData.CopyTo(realBytes,lenghtBytes.Length);
        return realBytes;
    }

    public void ParseData(int lenght,Action<string> callback)
    {
        startLenght += lenght;
        while (true)
        {
            if(startLenght < 4) return;

            int count = BitConverter.ToInt32(dataBytes, 0);
            if ((startLenght - count) >= 4)
            {
                //byte[] realBytes = new byte[count];
                //dataBytes.CopyTo(realBytes, 4);
                //Debug.Log("接收到的数据 " + Encoding.UTF8.GetString(dataBytes,4,count));
                startLenght = startLenght - count - 4;

                callback(Encoding.UTF8.GetString(dataBytes, 4, count));
            }
            else
            {
                return;
            }
        }
    }
}
