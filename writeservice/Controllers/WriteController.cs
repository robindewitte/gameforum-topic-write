using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace writeservice.Controllers
{
    public class WriteController : Controller
    {
       public void DoWriteStuffMock()
       {
            Console.WriteLine(DateTime.Now.ToString());
       }
    }
}
