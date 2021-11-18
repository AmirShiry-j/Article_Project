using Article_Project.Entities.Entity;
using Article_Project.Dtoes.Dto.Post;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Tools
{
    public class Pagination
    {
        Controller controller;
        public Pagination(Controller controller)
        {
            this.controller = controller;
        }

        public void SetPagination(List<ShortDisplayPostDto> posts , int Page=0)
        {
            if (Page == 0)
            {
                Page = 1;
            }
            controller.ViewBag.pageCurrent = Page;

            int Limit = 10;
            int PostStart = (int)(Page - 1) * Limit;
            int CountPosts = posts.Count();
            int BaghiMande = CountPosts % Limit;

            int CountPages = BaghiMande == 0 ? CountPosts / Limit : (CountPosts / Limit) + 1;

            if (BaghiMande != 0)
            {
                double Haf = (Limit / 4.0);
                if (BaghiMande < Haf)
                {
                    CountPages--;
                }
            }

            controller.ViewBag.CountPages = CountPages == 0 ? 1 : CountPages;
            controller.ViewBag.CountPosts = CountPosts;

            List<ShortDisplayPostDto> PostsPage;

            if (CountPages == Page)
            {
                PostsPage = posts.Skip(PostStart).Take(Limit + BaghiMande).ToList();
            }
            else
            {
                PostsPage = posts.Skip(PostStart).Take(Limit).ToList();
            }

            controller.ViewData["PostsPage"] = PostsPage;
        }
    }
}
