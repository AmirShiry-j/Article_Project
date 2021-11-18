using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Tools
{
    public class ServiceCommon
    {
        public string GetDescription(string Texts)
        {
            return Texts.Split("#*$", StringSplitOptions.RemoveEmptyEntries)[0].Substring(0, 100);
        }
        public string GetNameFirstImage(string ImgNames)
        {
            return ImgNames.Split("#*$", StringSplitOptions.RemoveEmptyEntries)[0];
        }
        public void DeleteImageProfile(string ImageProfileName)
        {

        }
    }
}
