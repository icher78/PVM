﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using PolandVisaAuto.Annotations;
using pvhelper;

namespace PolandVisaAuto
{
    public enum VisaEntityType
    {
        New = 0,
        Completed = 1
    }

    public class VisaTask : INotifyPropertyChanged
    {
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public string CityV {
            get
            {
                switch (CategoryCode)
                {
                    case "1"://National Visa
                        return "(N)" + City;
                    case "2"://Shengen
                        return "(S)" + City;
                }
                return City;
            }
        }

        public string City { get; set; }
        [Browsable(false)]
        public string CityCode { get; set; }
        public string Purpose { get; set; }
        [Browsable(false)]
        public string PurposeCode { get; set; }

        public int CountAdult { get; set; }
        public int CountChild { get; set; }

        public string Category { get; set; }
        [Browsable(false)]
        public string CategoryCode { get; set; }

        public string Receipt { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PassportEndDate { get; set; }

        public string Status { get; set; }
        [Browsable(false)]
        public string StatusCode { get; set; }
        
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string ArrivalDt { get; set; }
        public string Nationality { get; set; }
        public int Priority { get; set; }
        [XmlIgnoreAttribute]
        [Browsable(false)]
        public string PriorityStr { get { return Const.GetListPriority()[Priority]; } }

        public string RedLine { get; set; }
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public DateTime RedLineDt {
            get
            {
                return DateTime.ParseExact(RedLine, Const.DateFormat, CultureInfo.InvariantCulture);
            }
        }

        public string GreenLine { get; set; }
        [Browsable(false)]
        [XmlIgnoreAttribute]
        public DateTime GreenLineDt
        {
            get
            {
                if (GreenLine == null)
                    return DateTime.MinValue;
                return DateTime.ParseExact(GreenLine, Const.DateFormat, CultureInfo.InvariantCulture);
            }
        }

        public string RegistrationInfo { get; set; }

        public static bool IsValidEmailAddress(string email)
        {
            try
            {
                MailAddress ma = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(string pass)
        {
            return pass.Length >= 8;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static void Save(BindingList<VisaTask> tasks, VisaEntityType type)
        {
            string filemane = getFileName(type);
            using (FileStream fs = new FileStream(AssemblyDirectory + filemane, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(BindingList<VisaTask>));
                xs.Serialize(fs, tasks);
            }
        }

        public static void SaveDataToFolder(BindingList<VisaTask> tasks, string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(BindingList<VisaTask>));
                xs.Serialize(fs, tasks);
            }
        }
        
        public void Save()
        {
            string filemane = createFileName();
            string dir = Path.Combine(AssemblyDirectory, Const.DELETEDTASKS);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (FileStream fs = new FileStream(Path.Combine(dir, filemane), FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(VisaTask));
                xs.Serialize(fs, this);
            }
        }

        public static VisaTask DeSerialize(string filePath)
        {
            VisaTask task = null;
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(VisaTask));
                    task = (VisaTask)xs.Deserialize(fs);
                }
            }
            return task;
        }

        public static BindingList<VisaTask> DeSerialize(VisaEntityType type)
        {
            string filemane = getFileName(type);
            BindingList<VisaTask> tasks = new BindingList<VisaTask>();
            if (File.Exists(AssemblyDirectory + filemane))
            {
                using (FileStream fs = new FileStream(AssemblyDirectory + filemane, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(BindingList<VisaTask>));
                    tasks = (BindingList<VisaTask>) xs.Deserialize(fs);
                }
            }
            return tasks;
        }

        public string GetFullNameAsFileName()
        {
            return string.Format("{0}_{1}.txt", LastName, Name);
        }

        private static string getFileName(VisaEntityType type)
        {
            return type == VisaEntityType.New ? "\\data.xml" : "\\completedData.xml";
        }
        
        private string createFileName()
        {
            return string.Format("{0}_{1}_{2}.xml", Name, LastName, DateTime.Now.ToString(Const.DateFormatForFile, CultureInfo.InvariantCulture));
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name + " " + LastName);
            sb.AppendLine(string.Format("Крайняя дата подачи заявки от {2} до {0}, приоритет {1}",RedLine, PriorityStr, GreenLine));
            sb.AppendLine("");
            sb.AppendLine("Город: " + City);
            sb.AppendLine("Мета візиту: " + Purpose);
            sb.AppendLine(string.Format("Кількість заявників: {0}, К-сть дітей {1}",CountAdult,CountChild));
            sb.AppendLine("Візова категорія: " + Category);
            sb.AppendLine("Номер квитанції: " + Receipt);
            sb.AppendLine("Email: " + Email);
            sb.AppendLine("Пароль: " + Password);
            return sb.ToString();
        }

        public VisaTask Clone()
        {
            VisaTask vt = new VisaTask();
            vt.ArrivalDt = ArrivalDt;
            vt.Category = Category;
            vt.CategoryCode = CategoryCode;
            vt.City = City;
            vt.CityCode = CityCode;
            vt.CountAdult = CountAdult;
            vt.CountChild = CountChild;
            vt.Dob = Dob;
            vt.Email = Email;
            vt.LastName = LastName;
            vt.Name = Name;
            vt.Nationality = Nationality;
            vt.PassportEndDate = PassportEndDate;
            vt.Password = Password;
            vt.Priority = Priority;
            vt.Purpose = Purpose;
            vt.PurposeCode = PurposeCode;
            vt.Receipt = Receipt;
            vt.RedLine = RedLine;
            vt.GreenLine = GreenLine;
            vt.RegistrationInfo = RegistrationInfo;
            vt.Status = Status;
            vt.StatusCode = StatusCode;
            return vt;
        }
    }

    public class VisaComparer : IComparer<VisaTask>
    {
        public int Compare(VisaTask x, VisaTask y)
        {
            if (x.RedLineDt > y.RedLineDt)
                return 1;
            if (x.RedLineDt < y.RedLineDt)
                return -1;
            if (x.Priority > y.Priority)
                return -1;
            if (x.Priority < y.Priority)
                return 1;
            return 0;
        }
    }
}
