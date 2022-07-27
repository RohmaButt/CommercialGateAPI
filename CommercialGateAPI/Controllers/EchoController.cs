using System;
using System.IO;
using System.Text;
using System.Web.Http;

namespace CommercialGateAPI.Controllers
{
    [RoutePrefix("api/Echo")]
    public class EchoController : ApiController
    {
        [HttpGet]
        [Route("GetFile")]
        public IHttpActionResult GetFile()
        {
            FileWrite();
            return Ok("Yo bro!!  No Authentication/Authorization testing.");
        }

        [HttpPost]
        [Route("Postfile")]
        public IHttpActionResult Postfile()
        {
            FileWrite();
            return Ok("Yo bro!!  No Authentication/Authorization testing.");
        }


        private void FileWrite()
        {
            string fileName = @"D:\POCs\RestCallFromScheduler\FromtaskFile.txt";// "C:\Temp\Mahesh.txt";
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes("New Text File...");
                    fs.Write(title, 0, title.Length);
                    byte[] author = new UTF8Encoding(true).GetBytes("testing string in file");
                    fs.Write(author, 0, author.Length);
                }

                // Open the stream and read it back.    
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

        }
    }


}
