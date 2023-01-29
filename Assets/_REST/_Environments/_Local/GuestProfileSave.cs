using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Environments._Local;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _Environments._Local
{
    public class GuestProfileSave
    {
        public static string GUESTUUID;
        public static bool FILEFOUND = false;
        public static void SaveFile()
        {
            Guid GuestUUID = Guid.NewGuid();

            string destination = Application.persistentDataPath + "/guestuuidsave.dat";
            FileStream file;

            if (File.Exists(destination)) 
            {
                file = File.OpenWrite(destination);
                UnityEngine.Debug.Log("Guest UUID Created");
            } 
            else
            {
                file = File.Create(destination);
            }

            GuestProfileData data = new GuestProfileData(GuestUUID.ToString());
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }

        public static void LoadFile()
        {
            string destination = Application.persistentDataPath + "/guestuuidsave.dat";
            FileStream file;

            if (File.Exists(destination))
            {
                file = File.OpenRead(destination);
                FILEFOUND = true;
            }
            else
            {
                UnityEngine.Debug.LogError("File not found");
                FILEFOUND = false;
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            GuestProfileData data = (GuestProfileData)bf.Deserialize(file);
            file.Close();

            GUESTUUID = data.GUESTUUID;

            UnityEngine.Debug.Log(GUESTUUID);
        }
    }
}

