using System;
using System.Collections.Generic;
using System.IO;
using Serializer = System.Xml.Serialization;
using XmlManager.Models;

namespace XmlManager.Logic.XmlSerializer
{
    public class XmlSerializer
    {
        public static string GetSqlConnection(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.SqlConnectionString.connection;
            }
            catch (Exception ex) { throw; }
        }

        public static string GetDatabaseName(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.DataBaseName.Name;
            }
            catch (Exception ex) { throw; }
        }

        public static string GetLocalPathToDB(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.LocalPathToDB.Path;
            }
            catch (Exception ex) { throw; }
        }

        public static string GetRegionalParameters(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.RegionalParameters.Name;
            }
            catch (Exception ex) { throw; }
        }

        public static List<Operation> GetOperations(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.Operation;
            }
            catch (Exception ex) { throw; }
        }

        public static Process GetProcess(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.Process;
            }
            catch (Exception ex) { throw; }
        }


        public static EquipmentInfo GetEquipmentInfo(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.EquipmentInfo;
            }
            catch (Exception ex) { throw; }
        }

        public static MaterialParameters GetMaterialParameters(string pathToXml)
        {
            try
            {
                FBConfig fbConfig;
                Serializer.XmlSerializer formatter = new Serializer.XmlSerializer(typeof(FBConfig));
                using (FileStream fs = new FileStream(pathToXml, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fbConfig = (FBConfig)formatter.Deserialize(fs);
                }
                return fbConfig.MaterialParameters;
            }
            catch (Exception ex) { throw; }
        }

    }
}
