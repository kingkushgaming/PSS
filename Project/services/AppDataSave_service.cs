using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Security.Principal;
using Project.objects;
using Project.scripts;
using System.Diagnostics;

namespace Project.services
{
    class AppDataSave_service
    {
        public static readonly string path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\{Assembly.GetExecutingAssembly().GetName().Name}";

        public AppDataSave_service()
        {
            if (!Path.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void SaveUserLoginData(object obj)
        {
            string PATH = Path.Combine(path, "userdata.json");
            string json = JsonConvert.SerializeObject(obj);

            Crypt sec = new("userdata.json");
            var secureArray = sec.Encrypt(json);


            using (FileStream fs = new FileStream(PATH, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(secureArray, 0, secureArray.Length);
            }
        }

        public UserDataLogin LoadUserLoginData()
        {
            string PATH = Path.Combine(path, "userdata.json");
            if (!File.Exists(PATH))
                return null;

            var data = File.ReadAllBytes(PATH);
            Crypt sec = new("userdata.json");
            string dat = sec.Decrypt(data);
            Debug.WriteLine(dat);
            return JsonConvert.DeserializeObject<UserDataLogin>(dat);
        }


        public void DeleteUserLoginData()
        {
            string PATH = Path.Combine(path, "userdata.json");

            if (!Path.Exists(PATH)) { return; }

            File.Delete(PATH);
        }
    }
}
