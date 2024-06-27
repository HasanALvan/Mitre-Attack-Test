using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Web_Project.Database;
using Web_Project.Models;

namespace Web_Project.Controllers
{
    public class AttackController : Controller
    {
        // GET: Attack

        static string userName = Environment.UserName;

        public List<Attack> GetAttacks()
        {
            AttackDB getData = new AttackDB();
            List<Attack> attacks = new List<Attack>();
            attacks.Add(getData.getAttack("T1120"));
            attacks.Add(getData.getAttack("T1518"));
            return attacks;
        }
        public ActionResult Attacks()
        {
            if (Session["ID"] != null)
            {
                ViewBag.attacks = GetAttacks();
                return View("Attacks");
            }
            return View("~/Views/Account/Login.cshtml");
        }

        public ActionResult Download(string ID)
        {
            AttackDB attackData = new AttackDB();
            Attack attack = attackData.getAttack(ID);
            string file = attack.test_loc;
            string contentType = "application/exe";
            ViewBag.ErrorMessage = "";
            string userID = Session["ID"].ToString();
            object args = new object[2] { ID, userID};
            Thread RunAttack = new Thread(runAttack);
            RunAttack.Start(args);
            return File(file, contentType, Path.GetFileName(file));
        }

        public ActionResult Result(string ID)
        {
            if (ID == "T1120")
                return T1120();
            return T1518();
        }

        public ActionResult DownloadResult(string ID)
        {
            string file, contentType;
            if (ID == "T1120")
            {
                file = "C:\\Users\\" + userName + "\\Mitre Attack\\T1120\\Result.txt";
                contentType = "text/plain";
                return File(file, contentType, Path.GetFileName(file));
            }
            file = "C:\\Users\\" + userName + "\\Mitre Attack\\T1518\\Result.txt";
            contentType = "text/plain";
            return File(file, contentType, Path.GetFileName(file));
        }
        public ActionResult T1120()
        {
            string location = "C:\\Users\\" + userName + "\\Mitre Attack\\T1120\\Result.txt";
            if (System.IO.File.Exists(location))
            {
                //ViewBag.Attack = System.IO.File.ReadAllText(location, Encoding.UTF8).ToString();
                string line;
                StreamReader file = new StreamReader(location);
                var fileLines = new List<string>();
                while ((line = file.ReadLine()) != null)
                    fileLines.Add(line);
                ViewBag.Attack = fileLines;
                return View("T1120");
            }
            ViewBag.attacks = GetAttacks();
            return View("Attacks");
        }
        public ActionResult T1518()
        {
            string location = "C:\\Users\\" + userName + "\\Mitre Attack\\T1518\\Result.txt";
            if (System.IO.File.Exists(location))
            {
                string line;
                StreamReader file = new StreamReader(location);
                var fileLines = new List<string>();
                while ((line = file.ReadLine()) != null)
                    fileLines.Add(line);
                ViewBag.Attack = fileLines;
                return View("T1518");
            }
            ViewBag.attacks = GetAttacks();
            return View("Attacks");
        }


        private void runAttack(object argArray)
        {
            try
            {
                Array parameters = new object[2];
                parameters = (Array)argArray;

                string ID = parameters.GetValue(0).ToString();
                string userID = parameters.GetValue(1).ToString();
                AttackDB attackData = new AttackDB();
                Attack attack = attackData.getAttack(ID);
                string location = "C:\\Users\\" + userName + "\\Downloads\\" + attack.ID + ".exe";
                string attackResult = "";

                while (!System.IO.File.Exists(location))
                    Thread.Sleep(2000);
                
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = location;
                info.Arguments = "";
                info.WindowStyle = ProcessWindowStyle.Normal;
                info.WorkingDirectory = Path.GetDirectoryName(location);
                Process p = Process.Start(info);
                p.StartInfo.UseShellExecute = false;

                string resultLocation = "C:\\Users\\" + userName + "\\Mitre Attack\\" + attack.ID + "\\Result.txt";

                do
                {
                    try
                    {
                        attackResult = System.IO.File.ReadAllText(resultLocation, Encoding.UTF8);
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                } while (true);

                
                attackData.insertTest(attack, attackResult,userID);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Controller "+e.Message);
            }
        }

    }
}