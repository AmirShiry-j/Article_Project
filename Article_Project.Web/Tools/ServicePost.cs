using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Tools
{
    public class ServicePost
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePost(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void DeleteImagePost(string ImageName)
        {
            if (ImageName != null)
            {
                var ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/ImgPost/", ImageName);
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
            }
        }

        public string SetSubject(string Subject)
        {
            string TrueSubject = "غیره";

            switch (Subject)
            {
                case "هوش مصنوعی":
                    {
                        TrueSubject = "هوش مصنوعی";

                    }
                    break;
                case "کسب و کار":
                    {
                        TrueSubject = "کسب و کار";

                    }
                    break;
                case "روانشناسی":
                    {
                        TrueSubject = "روانشناسی";

                    }
                    break;
                case "فناوری":
                    {
                        TrueSubject = "فناوری";

                    }
                    break;
                case "استارت آپ":
                    {
                        TrueSubject = "استارت آپ";

                    }
                    break;
                case "برنامه نویسی":
                    {
                        TrueSubject = "برنامه نویسی";

                    }
                    break;
                case "موفقیت":
                    {
                        TrueSubject = "موفقیت";

                    }
                    break;
                case "سینما":
                    {
                        TrueSubject = "سینما";
                    }
                    break;
                case "موسیقی":
                    {
                        TrueSubject = "موسیقی";
                    }
                    break;
                case "هنر":
                    {
                        TrueSubject = "هنر";
                    }
                    break;
            }

            return TrueSubject;
        }
        public string GetSubject(string Subject)
        {
            string TrueSubject = null;

            switch (Subject)
            {
                case "هوش مصنوعی":
                    {
                        TrueSubject = "هوش مصنوعی";

                    }
                    break;
                case "کسب و کار":
                    {
                        TrueSubject = "کسب و کار";

                    }
                    break;
                case "روانشناسی":
                    {
                        TrueSubject = "روانشناسی";

                    }
                    break;
                case "فناوری":
                    {
                        TrueSubject = "فناوری";

                    }
                    break;
                case "استارت آپ":
                    {
                        TrueSubject = "استارت آپ";

                    }
                    break;
                case "برنامه نویسی":
                    {
                        TrueSubject = "برنامه نویسی";

                    }
                    break;
                case "موفقیت":
                    {
                        TrueSubject = "موفقیت";

                    }
                    break;
                case "سینما":
                    {
                        TrueSubject = "سینما";
                    }
                    break;
                case "موسیقی":
                    {
                        TrueSubject = "موسیقی";
                    }
                    break;
                case "هنر":
                    {
                        TrueSubject = "هنر";
                    }
                    break;
                case "غیره":
                    {
                        TrueSubject = "غیره";
                    }
                    break;
            }

            return TrueSubject;
        }
        public string[] GetArryOfString(string Reshte)
        {
            string[] Arry = Reshte.Split("#*$");

            return Arry;
        }
        public string[] GetCorrectOrders(string[] Orders, string[] Texts)
        {
            int imgNum = 1;
            for (int i = 0; i < Orders.Length; i++)
            {
                if (Orders[i].Contains('p'))
                {
                    Orders[i] = Orders[i][0].ToString() + imgNum;
                    imgNum++;
                }
            }

            int txtNum = 1;
            int menha = 0;
            for (int i = 0; i < Orders.Length; i++)
            {
                if (Orders[i].Contains('t'))
                {
                    if (Texts[i - menha] != "")
                    {
                        Orders[i] = Orders[i][0].ToString() + txtNum;
                    }
                    else
                    {
                        Orders[i] = "";
                    }

                    txtNum++;
                }
                else
                {
                    menha++;

                }
            }

            string TempArry = string.Join("#*$", Orders);

            string[] OkArryOrders = TempArry.Split("#*$", StringSplitOptions.RemoveEmptyEntries);

            txtNum = 1;
            for (int i = 0; i < OkArryOrders.Length; i++)
            {
                if (OkArryOrders[i].Contains('t'))
                {
                    OkArryOrders[i] = OkArryOrders[i][0].ToString() + txtNum;
                    txtNum++;
                }
            }

            return OkArryOrders;
        }
        public string GetCorrectionTexts(string Texts)
        {
            string[] Es = Texts.Split("#*$");
            foreach (var e in Es)
            {
                string s = e;
            }
            return Es.ToString();
        }
        public string GetCurrentTime(DateTime timeCreate)
        {
            int tempTime;

            tempTime = (DateTime.Now - timeCreate).Days;
            if (tempTime >= 30)
            {
                return tempTime / 30 + " ماه پیش";
            }
            if (tempTime != 0)
            {
                return tempTime + " روز پیش";
            }

            tempTime = (DateTime.Now - timeCreate).Hours;
            if (tempTime != 0)
            {
                return tempTime + " ساعت پیش";
            }

            tempTime = (DateTime.Now - timeCreate).Minutes;
            if (tempTime != 0)
            {
                return tempTime + " دقیقه پیش";
            }

            tempTime = (DateTime.Now - timeCreate).Seconds;
            return tempTime + " ثانیه پیش";
        }
        public string ToShamsi(DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }
    }
}
