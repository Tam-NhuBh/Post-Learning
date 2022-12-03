﻿using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class FavController : BaseController
    {
        // GET: Admin/Fav
        FavBLL bll = new FavBLL();
        public ActionResult UpdateFav()
        {
            FavDTO dto = new FavDTO();
            dto = bll.GetFav();
            return View(dto);
        }
        [HttpPost]
        public ActionResult UpdateFav(FavDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProcessState = General.Messages.EmptyArea;
            }
            else
            {
                if (model.FavImage != null)
                {
                    string favname = "";
                    HttpPostedFileBase postedfile = model.FavImage;
                    Bitmap FavImage = new Bitmap(postedfile.InputStream);
                    Bitmap resizeImage = new Bitmap(FavImage, 100, 100);
                    string ext = Path.GetExtension(postedfile.FileName);
                    //kiem tra file dau vao
                    if (ext==".ico" || ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                    {
                        string uniqueNumber = Guid.NewGuid().ToString(); 
                        favname = uniqueNumber + postedfile.FileName;
                        resizeImage.Save(Server.MapPath("~/Areas/Admin/Content/FavImage/" + favname));
                        model.Fav = favname;
                    }
                    else
                        ViewBag.ProcessState = General.Messages.ExtensionError;
                }
                if (model.LogoImage != null)
                {
                    string logoname = "";
                    HttpPostedFileBase postedfilelogo = model.LogoImage;
                    Bitmap LogoImage = new Bitmap(postedfilelogo.InputStream);
                    Bitmap resizeImage = new Bitmap(LogoImage, 100, 100);
                    string ext = Path.GetExtension(postedfilelogo.FileName);
                    //kiem tra file dau vao
                    if (ext == ".ico" || ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
                    {
                        string logounique = Guid.NewGuid().ToString();
                        logoname = logounique + postedfilelogo.FileName;
                        resizeImage.Save(Server.MapPath("~/Areas/Admin/Content/FavImage/" + logoname));
                        model.Logo = logoname;
                    }
                    else
                        ViewBag.ProcessState = General.Messages.ExtensionError;
                }
                FavDTO returndto = new FavDTO();
                returndto = bll.UpdateFav(model);
                if( model.FavImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Areas/Admin/Content/FavImage/" + returndto.Fav)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Content/FavImage/" + returndto.Fav));
                    }
                }
                if (model.LogoImage != null)
                {
                    if (System.IO.File.Exists(Server.MapPath("~/Areas/Admin/Content/FavImage/" + returndto.Logo)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Content/FavImage/" + returndto.Logo));
                    }
                }
                ViewBag.ProcessState = General.Messages.UpdateSuccess;
            }
            return View(model);
        }
    }
}