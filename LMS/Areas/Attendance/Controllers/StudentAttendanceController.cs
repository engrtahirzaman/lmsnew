using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Areas.Attendance.Controllers
{
    public class StudentAttendanceController : Controller
    {
        CMSEntities db = new CMSEntities();
        // GET: Attendance/StudentAttendance
        public async Task<ActionResult> AllAttendance()
        {
            int StudentID = Convert.ToInt32(Session["StudentID"]);
            var studentAttendances = db.StudentAttendances.Where(t => t.StudentID == StudentID).OrderByDescending(t => t.SessionID).ToListAsync();
            return View(await studentAttendances);
        }
    }
}