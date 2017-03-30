using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Team4Clock
{
    /// <summary>
    /// This class is responsible for handling Alarm reading and writing.
    /// 
    /// Note: this is kind of a hack. This should be made into a singleton, and maybe 
    /// adapted into an app service.
    /// </summary>
    class AlarmIO
    {
        private string _saveDir;
        private string _alarmFile;

        /// <summary>
        /// Base constructor. Sets up save dir and creates it, if need be.
        /// 
        /// Todo: This needs exception handling (e.g. what if we can't write the dir?)
        /// </summary>
        public AlarmIO()
        {
            //string saveDirBase = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string saveDirBase = Path.GetTempPath();
            string saveDirName = "Team4Clock";
            _saveDir = Path.Combine(saveDirBase, saveDirName);
            _alarmFile = Path.Combine(_saveDir, "alarms.bin");

            Directory.CreateDirectory(_saveDir);
        }

        public void WriteAlarmList(List<Alarm> alarmList)
        {
            try
            {
                using (Stream stream = File.Open(_alarmFile, FileMode.Create)) {
                    BinaryFormatter binForm = new BinaryFormatter();
                    binForm.Serialize(stream, alarmList);
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine("[ERROR] IOException caught when trying to write alarm list");
            }
        }

        public List<Alarm> ReadAlarmList()
        {
            try
            {
                using (Stream stream = File.Open(_alarmFile, FileMode.Open))
                {
                    BinaryFormatter binForm = new BinaryFormatter();
                    if (stream.Length > 0)
                    {
                        var retList = (List<Alarm>)binForm.Deserialize(stream);
                        return retList;
                    }
                    return null;
;                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
    }
}
