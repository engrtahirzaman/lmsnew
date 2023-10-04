using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMS.Models;


    public sealed class Methods
    {
        private static CMSEntities db = new CMSEntities();

        // Get student last semester no.
        public static int GetStudentCurrentSemesterNumber(int studentID)
        {
            int lastSemester = db.StudentSemesters
                .Where(s => s.StudentID == studentID)
                .OrderByDescending(s => s.SemesterNo)
                .Select(s => s.SemesterNo)
                .FirstOrDefault();
            return lastSemester;
        }

        public static int GetStudentCurrentSessionID(int studentID)
        {
            int currentSessionID = db.StudentSemesters
                .Where(s => s.StudentID == studentID)
                .OrderByDescending(s => s.SessionID)
                .Select(s => s.SessionID)
                .FirstOrDefault();
            return currentSessionID;
        }

        // Get student batchID
        public static int GetStudentBatchID(int studentID)
        {
            int batchID = db.Students.Find(studentID).BatchID;
            return batchID;
        }
    }

